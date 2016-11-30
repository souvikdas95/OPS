using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.DistanceMatrix.Request;
using GoogleMapsApi.Entities.DistanceMatrix.Response;

namespace OPS
{
    public partial class Customer_Home_Product_Open : Form
    {
        private CProduct item;
        private List<CLocation> location_list;
        private Size imgSize;
        private Form prev;

        public Customer_Home_Product_Open(CProduct arg)
        {
            item = arg;
            prev = null;

            InitializeComponent();
        }

        public Customer_Home_Product_Open(CProduct arg, Form arg2)
        {
            item = arg;
            prev = arg2;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = prev.Location;

            InitializeComponent();
        }

        private async void Customer_Home_Product_Open_Load(object sender, EventArgs e)
        {            
            // Fill Default Fields
            textBox_Name.Text = item.name;
            comboBox_Catagory.Items.Add(await CCatagory.Retrieve(item.catagory_id));
            comboBox_Catagory.SelectedIndex = 0;
            richTextBox_Description.Text = item.description;
            progressBar_Rating.Value = (Int32)(item.rating * 100.0);
            location_list = await CLocation.RetrieveLocationList();
            comboBox_Pincode.Items.AddRange(location_list.ToArray());
            imgSize = item.image.Size;
            pictureBox_Picture.Image = item.image;

            // Fill Product Fields
            List<CProduct_Field> extlist = await CProduct_Field.RetrieveProductFieldList(item.id);
            foreach (CProduct_Field x in extlist)
            {
                tableLayoutPanel3.RowCount++;
                tableLayoutPanel3.RowStyles.Add(new RowStyle());

                Label extLabel = new Label()
                {
                    TabIndex = 0,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),
                    Text = x.field_name,
                    Size = new Size(16, 16),
                    Dock = DockStyle.Fill
                };
                TextBox extTextBox = new TextBox()
                {
                    TabIndex = 1,
                    Text = x.field_value,
                    Size = new Size(16, 16),
                    Dock = DockStyle.Fill,
                    ReadOnly = true
                };

                tableLayoutPanel3.Controls.Add(extLabel, 0, tableLayoutPanel3.RowCount - 1);
                tableLayoutPanel3.Controls.Add(extTextBox, 1, tableLayoutPanel3.RowCount - 1);
            }

            // Retrieve Sellers With Products
            List<CSeller_Inventory> list1 = await CSeller_Inventory.RetrieveSellerInventoryListByProduct(item.id);
            if(list1.Count < 1)
            {
                comboBox_Pincode.Enabled = false;
                button_Check.Enabled = false;
                textBox_Quantity.Enabled = false;
                button_AddToCart.Enabled = false;
            }
            dataGridView_SellerList.Columns.Add("seller_id", "Seller ID");
            dataGridView_SellerList.Columns.Add("name", "Seller Name");
            dataGridView_SellerList.Columns.Add("price", "Price");
            dataGridView_SellerList.Columns.Add("quantity", "Available");
            dataGridView_SellerList.Columns.Add("warranty", "Warranty");
            dataGridView_SellerList.Columns.Add("shipping_time", "Shipping Time");
            // Invisible Containers inside Data Grid
            dataGridView_SellerList.Columns.Add("extra_1", "extra_1");
            dataGridView_SellerList.Columns.Add("extra_2", "extra_2");
            dataGridView_SellerList.Columns["extra_1"].Visible = false;
            dataGridView_SellerList.Columns["extra_2"].Visible = false;
            for (Int16 i = 0; i < list1.Count; ++i)
            {
                CSeller_Inventory x = list1[i];
                CUser_Seller y = await CUser_Seller.Retrieve(x.seller_id);
                dataGridView_SellerList.Rows.Add
                (
                    new object[]
                    {
                        x.seller_id,
                        y.name,
                        x.price,
                        x.quantity,
                        x.warranty,
                        null,
                        x,
                        y,
                    }
                );
            }

            // Disable Form Auto Size Later
            Size temp = this.Size;
            this.AutoSize = false;
            this.Size = temp;

            // Enable Size Manager & Force Reset Size
            Customer_Home_Product_Open_SizeChanged_Custom(null, null);
            this.SizeChanged += new EventHandler(Customer_Home_Product_Open_SizeChanged_Custom);
        }

        private async void button_Check_Click(object sender, EventArgs e)
        {
            // Check if Pincode Selected
            if(comboBox_Pincode.SelectedIndex < 0)
            {
                MessageBox.Show("Invalid Pincode!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Calculate & Populate Duration & Sort Seller List
            DistanceMatrixRequest y = new DistanceMatrixRequest();
            y.ApiKey = Program.szGoogleMapsAPI_key;
            y.Origins = new string[] { comboBox_Pincode.Text };
            y.Mode = DistanceMatrixTravelModes.driving;
            List<string> Destinations = new List<string>();
            foreach (DataGridViewRow x in dataGridView_SellerList.Rows)
            {
                CSeller_Inventory temp = (CSeller_Inventory)(x.Cells["extra_1"].Value);
                Destinations.Add(temp.pincode.ToString());
            }
            y.Destinations = Destinations.ToArray<string>();
            DistanceMatrixResponse z = await GoogleMapsApi.GoogleMaps.DistanceMatrix.QueryAsync(y);
            for (int i = 0; i < z.Rows.ElementAt<Row>(0).Elements.Count<Element>(); ++i)
            {
                Element x = z.Rows.ElementAt<Row>(0).Elements.ElementAt<Element>(i);
                dataGridView_SellerList.Rows[i].Cells["shipping_time"].Value = (x.Duration.Value.Days + 1) + " Day(s) " + (x.Duration.Value.Hours) + " Hour(s)";
            }
            dataGridView_SellerList.Sort(dataGridView_SellerList.Columns["shipping_time"], ListSortDirection.Ascending);
        }

        private async void button_AddToCart_Click(object sender, EventArgs e)
        {
            // Check Valid Quantity
            if (!CUtils.IsNumeric(textBox_Quantity.Text))
            {
                MessageBox.Show("Invalid Quantity!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int quantity = Int32.Parse(textBox_Quantity.Text);

            // Check Quantity Inventory Exceed
            int temp2 = (int)(dataGridView_SellerList.SelectedRows[0].Cells["quantity"].Value);
            if (quantity > temp2)
            {
                MessageBox.Show("Quantity Exceeds Seller's Capability!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Try Retrieve Existing Cart
            CUser_Seller temp = (CUser_Seller)(dataGridView_SellerList.SelectedRows[0].Cells["extra_2"].Value);
            CCustomer_Cart cartitem = await CCustomer_Cart.Retrieve(CUser.cur_user.id, item.id, temp.user_id);
            if (cartitem != null)
            {
                if (!(await cartitem.Commit(cartitem.quantity + quantity)))
                {
                    if (CUtils.LastLogMsg != null)
                        MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (!(await CCustomer_Cart.Register(CUser.cur_user.id,
                                                     item.id,
                                                     temp.user_id,
                                                     quantity)))  // Create New Cart
            {
                if (CUtils.LastLogMsg == null || CUtils.LastLogMsg.Equals("Inventory Already Exists!"))
                    MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Successfully Added to Cart!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Visible = false;
            this.Dispose();
        }

        private void Customer_Home_Product_Open_SizeChanged_Custom(object sender, EventArgs e)
        {
            Double ratio = (Double)(imgSize.Height) / (Double)(imgSize.Width);
            Int32 Width = pictureBox_Picture.Size.Width;
            Int32 Height = pictureBox_Picture.Size.Height;
            if (Width != tableLayoutPanel5.Size.Width)
                Width = tableLayoutPanel5.Size.Width;
            if (Height != (Int32)(Width * ratio))
                Height = (Int32)(Width * ratio);
            Int32 MaxHeight = tableLayoutPanel5.Size.Height
                            - label_Rating.Size.Height
                            - progressBar_Rating.Size.Height
                            - 16;
            if (Height > MaxHeight)
            {
                Height = MaxHeight;
                Width = (Int32) (Height * (1 / ratio));
            }
            if (pictureBox_Picture.Size.Width != Width || pictureBox_Picture.Size.Height != Height)
                pictureBox_Picture.Size = new Size(Width, Height);
        }

        private void Customer_Home_Product_Open_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (prev != null)
            {
                prev.StartPosition = this.StartPosition;
                prev.Location = this.Location;
                prev.Visible = true;
            }
        }
    }
}
