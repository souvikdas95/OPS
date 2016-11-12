using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPS
{
    public partial class Seller_Home_Product_Open : Form
    {
        private CProduct item;
        private CSeller_Inventory invitem;
        private List<CLocation> location_list;
        private Size imgSize;
        private Form prev;

        public Seller_Home_Product_Open(CProduct arg)
        {
            item = arg;
            prev = null;

            InitializeComponent();
        }

        public Seller_Home_Product_Open(CProduct arg, Form arg2)
        {
            item = arg;
            prev = arg2;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = prev.Location;

            InitializeComponent();
        }

        private async void Seller_Home_Product_Open_Load(object sender, EventArgs e)
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

            // Retrieve Inventory if Exists
            invitem = await CSeller_Inventory.Retrieve(CUser.cur_user.id, item.id);
            if (invitem != null)
            {
                textBox_Price.Text = invitem.price.ToString();
                textBox_Price.ReadOnly = true;
                foreach (CLocation x in comboBox_Pincode.Items)
                {
                    if (x.pincode == invitem.pincode)
                    {
                        comboBox_Pincode.SelectedItem = x;
                        break;
                    }
                }
                comboBox_Pincode.Enabled = false;
                textBox_Warranty.Text = invitem.warranty.ToString();
                textBox_Warranty.ReadOnly = true;
            }

            // Disable Form Auto Size Later
            Size temp = this.Size;
            this.AutoSize = false;
            this.Size = temp;

            // Enable Size Manager & Force Reset Size
            Seller_Home_Product_Open_SizeChanged_Custom(null, null);
            this.SizeChanged += new EventHandler(Seller_Home_Product_Open_SizeChanged_Custom);
        }

        private async void button_AddToInventory_Click(object sender, EventArgs e)
        {
            if (!CUtils.IsNumeric(textBox_Price.Text))
            {
                MessageBox.Show("Invalid Price!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!CUtils.IsNumeric(textBox_Quantity.Text))
            {
                MessageBox.Show("Invalid Quantity!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (comboBox_Pincode.SelectedIndex < 0)
            {
                MessageBox.Show("Invalid Pincode!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!CUtils.IsNumeric(textBox_Warranty.Text))
            {
                MessageBox.Show("Invalid Warranty!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(invitem != null)
            {
                if (!(await invitem.Commit(invitem.price,
                                          invitem.quantity + Int32.Parse(textBox_Quantity.Text),
                                          invitem.pincode,
                                          invitem.warranty)))
                {
                    if (CUtils.LastLogMsg != null)
                        MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (!(await CSeller_Inventory.Register(CUser.cur_user.id,
                                                        item.id,
                                                        Double.Parse(textBox_Price.Text),
                                                        Int32.Parse(textBox_Quantity.Text),
                                                        ((CLocation)comboBox_Pincode.SelectedItem).pincode,
                                                        Double.Parse(textBox_Warranty.Text))))
            {
                if (CUtils.LastLogMsg == null || CUtils.LastLogMsg.Equals("Inventory Already Exists!"))
                    MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Successfully Added to Inventory!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void comboBox_Pincode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar))
                return;
            ComboBox box = ((ComboBox)sender);

            string nonSelected = box.Text.Substring(0, box.Text.Length - box.SelectionLength);

            string text = nonSelected + e.KeyChar;
            bool matched = false;
            for (int i = 0; i < box.Items.Count; i++)
            {
                if (box.Items[i].ToString().StartsWith(text, true, null))
                {
                    matched = true;
                    break;
                }
            }
            e.Handled = !matched;
        }

        private void Seller_Home_Product_Open_SizeChanged_Custom(object sender, EventArgs e)
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

        private void Seller_Home_Product_Open_FormClosed(object sender, FormClosedEventArgs e)
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
