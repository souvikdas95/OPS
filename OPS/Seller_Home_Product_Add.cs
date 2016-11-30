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
    public partial class Seller_Home_Product_Add : Form
    {
        private CProduct item;
        private List<CLocation> location_list;
        private List<CCatagory> cat_list;
        private Size imgSize;
        private Boolean ButtonState;

        private class extFieldStruct
        {
            public Label label;
            public TextBox textBox;
            public RowStyle rowStyle;
            public extFieldStruct(Label label, TextBox textBox, RowStyle rowStyle)
            {
                this.label = label;
                this.textBox = textBox;
                this.rowStyle = rowStyle;
            }
            public extFieldStruct() { }
        };

        List<extFieldStruct> extField;

        public Seller_Home_Product_Add()
        {
            InitializeComponent();
        }

        private async void Seller_Home_Product_Add_Load(object sender, EventArgs e)
        {
            // Fill Default Fields
            item = null;
            extField = new List<extFieldStruct>();
            ButtonState = false;
            cat_list = await CCatagory.RetrieveCatagoryList(0);
            comboBox_Catagory.Items.AddRange(cat_list.ToArray());
            location_list = await CLocation.RetrieveLocationList();
            comboBox_Pincode.Items.AddRange(location_list.ToArray());
            imgSize = pictureBox_Picture.Size;

            // Disable Form Auto Size Later
            Size temp = this.Size;
            this.AutoSize = false;
            this.Size = temp;

            // Enable Size Manager & Force Reset Size
            Seller_Home_Product_Add_SizeChanged_Custom(null, null);
            this.SizeChanged += new EventHandler(Seller_Home_Product_Add_SizeChanged_Custom);
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

        private void Seller_Home_Product_Add_SizeChanged_Custom(object sender, EventArgs e)
        {
            Double ratio = (Double)(imgSize.Height) / (Double)(imgSize.Width);
            Int32 Width = pictureBox_Picture.Size.Width;
            Int32 Height = pictureBox_Picture.Size.Height;
            if (Width != tableLayoutPanel5.Size.Width)
                Width = tableLayoutPanel5.Size.Width;
            if (Height != (Int32)(Width * ratio))
                Height = (Int32)(Width * ratio);
            Int32 MaxHeight = tableLayoutPanel5.Size.Height
                            - button_Browse.Size.Height
                            - 16;
            if (Height > MaxHeight)
            {
                Height = MaxHeight;
                Width = (Int32) (Height * (1 / ratio));
            }
            if (pictureBox_Picture.Size.Width != Width || pictureBox_Picture.Size.Height != Height)
                pictureBox_Picture.Size = new Size(Width, Height);
        }

        private async void button_Main_Click(object sender, EventArgs e)
        {
            if (!ButtonState)
            {
                // Check Constraints
                if (String.IsNullOrWhiteSpace(textBox_Name.Text))
                {
                    MessageBox.Show("Invalid Name!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (comboBox_Catagory.SelectedIndex < 0)
                {
                    MessageBox.Show("Invalid Catagory!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (String.IsNullOrWhiteSpace(richTextBox_Description.Text))
                {
                    MessageBox.Show("Invalid Description!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (extFieldStruct x in extField)
                {
                    if(String.IsNullOrWhiteSpace(x.textBox.Text))
                    {
                        {
                            MessageBox.Show("Invalid " + x.label.Text + "!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                if(pictureBox_Picture.Image == null || pictureBox_Picture.Image.Equals(global::OPS.Properties.Resources.noimage))
                {
                    MessageBox.Show("Invalid Image!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Register Product
                if (!(await CProduct.Register(((CCatagory)comboBox_Catagory.SelectedItem).id,
                                              textBox_Name.Text,
                                              richTextBox_Description.Text,
                                              pictureBox_Picture.Image)))
                {
                    if (CUtils.LastLogMsg != null)
                        MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                item = await CProduct.Retrieve(((CCatagory)comboBox_Catagory.SelectedItem).id,
                                               textBox_Name.Text);
                if(item == null)
                {
                    if (CUtils.LastLogMsg != null)
                        MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Lock Product Specifications
                textBox_Name.ReadOnly = true;
                comboBox_Catagory.Enabled = false;
                richTextBox_Description.ReadOnly = true;
                foreach (extFieldStruct x in extField)
                {
                    x.textBox.ReadOnly = true;
                }
                button_Browse.Visible = false;

                // Change Form & Button State
                tableLayoutPanel2.Visible = true;
                button_Main.Text = "Add To Inventory";
                ButtonState = true;
            }
            else
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
                Console.WriteLine("QUAT=" + Int32.Parse(textBox_Quantity.Text));
                if (!(await CSeller_Inventory.Register(CUser.cur_user.id,
                                                 item.id,
                                                 Double.Parse(textBox_Price.Text),
                                                 Int32.Parse(textBox_Quantity.Text),
                                                 ((CLocation)comboBox_Pincode.SelectedItem).pincode,
                                                 Double.Parse(textBox_Warranty.Text))))
                {
                    if (CUtils.LastLogMsg.Equals("Inventory Already Exists!"))
                    {
                        CSeller_Inventory inv = await CSeller_Inventory.Retrieve(CUser.cur_user.id, item.id);
                        if (inv != null)
                        {
                            if (!(await inv.Commit(inv.price,
                                             inv.quantity + Int32.Parse(textBox_Quantity.Text),
                                             inv.pincode,
                                             inv.warranty)))
                            {
                                MessageBox.Show("Successfully Added to Inventory!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Visible = false;
                                this.Dispose();
                                return;
                            }
                        }
                        MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (CUtils.LastLogMsg != null)
                        MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                MessageBox.Show("Successfully Added to Inventory!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Visible = false;
                this.Dispose();
            }
        }

        private async void comboBox_Catagory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Erase Previous Fields
            foreach (extFieldStruct x in extField)
            {
                x.label.Visible = false;
                x.label.Dispose();
                x.textBox.Visible = false;
                x.textBox.Dispose();
                tableLayoutPanel3.RowStyles.Remove(x.rowStyle);
                tableLayoutPanel3.RowCount--;
            }

            // Fill Product Fields
            List<CCatagory_Field> extlist = await CCatagory_Field.RetrieveCatagoryFieldList(((CCatagory)comboBox_Catagory.SelectedItem).id);
            extField = new List<extFieldStruct>();
            extFieldStruct temp = null;
            foreach (CCatagory_Field x in extlist)
            {
                tableLayoutPanel3.RowCount++;
                temp = new extFieldStruct();
                temp.rowStyle = new RowStyle();
                tableLayoutPanel3.RowStyles.Add(temp.rowStyle);

                temp.label = new Label()
                {
                    TabIndex = 0,
                    Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))),
                    Text = x.field_name,
                    Size = new Size(16, 16),
                    Dock = DockStyle.Fill
                };
                temp.textBox = new TextBox()
                {
                    TabIndex = 1,
                    Size = new Size(16, 16),
                    Dock = DockStyle.Fill
                };
                extField.Add(temp);

                tableLayoutPanel3.Controls.Add(temp.label, 0, tableLayoutPanel3.RowCount - 1);
                tableLayoutPanel3.Controls.Add(temp.textBox, 1, tableLayoutPanel3.RowCount - 1);
            }
            extField.Reverse();
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Browse Image";
                dlg.Filter = "JPEG Image(*.jpg)|*.jpg|PNG Image(*.png)|*.png";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                Image temp = Image.FromFile(dlg.FileName);
                imgSize = temp.Size;
                pictureBox_Picture.Image = temp;
            }
        }
    }
}
