using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPS
{
    public partial class Checkout : Form
    {
        private List<CCustomer_Cart> cart_items;
        private List<CLocation> location_list;
        private Form prev;
        private Double GTotal;

        public Checkout(List<CCustomer_Cart> arg1, Form arg2, Double total)
        {
            InitializeComponent();

            cart_items = arg1;
            prev = arg2;
            GTotal = total;
            textBox_Cart_GrandTotal.Text = GTotal.ToString();
        }

        private async void Checkout_Load(object sender, EventArgs e)
        {
            location_list = await CLocation.RetrieveLocationList();
            comboBox_Pincode.Items.AddRange(location_list.ToArray());
        }

        private async void button_Pay_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(richTextBox_Street.Text))
            {
                MessageBox.Show("Invalid Street!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (comboBox_Pincode.SelectedIndex < 0)
            {
                MessageBox.Show("Invalid Pincode!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!Regex.IsMatch(textBox_Contact.Text, "^[0-9]{10}$"))
            {
                MessageBox.Show("Invalid Contact Number!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!Regex.IsMatch(textBox_DebitCard.Text,
                "^(?:4[0-9]{12}(?:[0-9]{3})?)$"
            ))
            {
                MessageBox.Show("Invalid Card Number!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (CCustomer_Cart x in cart_items)
            {
                await COrder.Register(x.customer_id,
                    x.product_id,
                    x.seller_id,
                    x.quantity,
                    Int64.Parse(textBox_Contact.Text),
                    richTextBox_Street.Text,
                    ((CLocation)(comboBox_Pincode.SelectedItem)).pincode,
                    GTotal);
                await CCustomer_Cart.Remove(x.customer_id, x.product_id, x.seller_id, true);

                // Update Sales in Product
                MySqlCommand tempcmd = new MySqlCommand();
                tempcmd.Connection = Program.conn;
                tempcmd.CommandText = "UPDATE `product` SET sales = sales + @quantity WHERE id = @product_id";
                tempcmd.Parameters.AddWithValue("@quantity", x.quantity);
                tempcmd.Parameters.AddWithValue("@product_id", x.product_id);
                await tempcmd.ExecuteNonQueryAsync();
                tempcmd.Dispose();
            }
            if (CUtils.LastLogMsg != null)
                MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                MessageBox.Show("Successfully Ordered!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                prev.Visible = true;
                this.Visible = false;
                this.Dispose();
            }
        }

        private void Checkout_FormClosed(object sender, FormClosedEventArgs e)
        {
            prev.Visible = true;
        }
    }
}
