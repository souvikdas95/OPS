﻿using System;
using System.Collections;
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
    public partial class Seller_Home : Form
    {
        // data
        private Form prev;
        private List<CCatagory> cat_list;

        public Seller_Home(Form frame)
        {
            prev = frame;
            this.Location = prev.Location;

            InitializeComponent();
        }

        private async void Seller_Home_Load(object sender, EventArgs e)
        {
            cat_list = await CCatagory.RetrieveCatagoryList(0);
            comboBox_Product_Search.Items.AddRange(cat_list.ToArray());
            //comboBox_Product_Search.AutoCompleteSource = AutoCompleteSource.ListItems;

            // Inventory Table Layout
            dataGridView_Inventory.Columns.Add("product_id", "Product ID");
            dataGridView_Inventory.Columns["product_id"].ReadOnly = true;
            dataGridView_Inventory.Columns.Add("product_name", "Product Name");
            dataGridView_Inventory.Columns["product_name"].ReadOnly = true;
            dataGridView_Inventory.Columns.Add("price", "Price");
            dataGridView_Inventory.Columns.Add("quantity", "Quantity");
            dataGridView_Inventory.Columns.Add("pincode", "Pincode");
            dataGridView_Inventory.Columns["pincode"].ReadOnly = true;
            dataGridView_Inventory.Columns.Add("warranty", "Warranty");
            // Invisible Containers inside Data Grid
            dataGridView_Inventory.Columns.Add("extra_1", "extra_1");
            dataGridView_Inventory.Columns.Add("extra_2", "extra_2");
            dataGridView_Inventory.Columns["extra_1"].Visible = false;
            dataGridView_Inventory.Columns["extra_2"].Visible = false;

            // Order Table Layout
            dataGridView_Orders.Columns.Add("order_id", "Order ID");
            dataGridView_Orders.Columns.Add("product_name", "Product Name");
            dataGridView_Orders.Columns.Add("price", "Price");
            dataGridView_Orders.Columns.Add("quantity", "Quantity");
            dataGridView_Orders.Columns.Add("address", "Address");
            dataGridView_Orders.Columns.Add("started_at", "Start Date");
            dataGridView_Orders.Columns.Add("eta", "ETA");
            dataGridView_Orders.Columns.Add("order_status", "Status");
            // Invisible Containers inside Data Grid
            dataGridView_Orders.Columns.Add("extra_1", "extra_1");   // order
            dataGridView_Orders.Columns.Add("extra_2", "extra_2");   // product
            dataGridView_Orders.Columns.Add("extra_3", "extra_3");   // seller_inventory
            dataGridView_Orders.Columns.Add("extra_4", "extra_4");   // location
            dataGridView_Orders.Columns["extra_1"].Visible = false;
            dataGridView_Orders.Columns["extra_2"].Visible = false;
            dataGridView_Orders.Columns["extra_3"].Visible = false;
            dataGridView_Orders.Columns["extra_4"].Visible = false;

            // Disable Form Auto Size Later
            Size temp = this.Size;
            this.AutoSize = false;
            this.Size = temp;
        }

        private void tabControl_MAIN_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;
            Font _textFont;

            // Get the item from the collection.
            TabPage _tabPage = tabControl_MAIN.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabControl_MAIN.GetTabRect(e.Index);

            _textBrush = new SolidBrush(Color.Black);
            if (e.State == DrawItemState.Selected)
            {
                _textFont = new Font(e.Font, FontStyle.Bold);
                g.FillRectangle(Brushes.White, e.Bounds);
                //e.DrawBackground();
            }
            else
            {
                _textFont = new Font(e.Font, FontStyle.Regular);
                g.FillRectangle(Brushes.GhostWhite, e.Bounds);
                //e.DrawBackground();
                
            }

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _textFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void button_Product_Add_Click(object sender, EventArgs e)
        {
            new Seller_Home_Product_Add().Visible = true;
        }

        private void Seller_Home_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void comboBox_Product_Search_KeyPress(object sender, KeyPressEventArgs e)
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

        private async void button_Product_Search_Click(object sender, EventArgs e)
        {
            if (comboBox_Product_Search.SelectedIndex < 0)
                return;

            String text = textBox_Product_Search.Text;
            CCatagory cat_obj = (CCatagory)comboBox_Product_Search.SelectedItem;
            List<CProduct> list = await CProduct.SearchProductList(cat_obj.id, text);
            dataGridView_Product.DataSource = new BindingSource(list, null);
            foreach (DataGridViewColumn x in dataGridView_Product.Columns)
            {
                if (x.Name.Equals("id"))
                {
                    x.DisplayIndex = 0;
                    x.HeaderText = "ID";
                }
                else if (x.Name.Equals("name"))
                {
                    x.DisplayIndex = 1;
                    x.HeaderText = "Name";
                }
                else if (x.Name.Equals("minprice"))
                {
                    x.DisplayIndex = 2;
                    x.HeaderText = "Price";
                }
                else if (x.Name.Equals("sales"))
                {
                    x.DisplayIndex = 3;
                    x.HeaderText = "Sales";
                }
                else if (x.Name.Equals("rating"))
                {
                    x.DisplayIndex = 4;
                    x.HeaderText = "Rating";
                }
                else if (x.Name.Equals("quantity"))
                {
                    x.DisplayIndex = 5;
                    x.HeaderText = "Available";
                }
                else
                {
                    x.Visible = false;
                }
            }
        }

        private void button_Product_Open_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow x in dataGridView_Product.SelectedRows)
            {
                new Seller_Home_Product_Open((CProduct)x.DataBoundItem).Visible = true;
            }
        }

        private async void button_Inventory_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow x in dataGridView_Inventory.Rows)
            {
                CSeller_Inventory y = (CSeller_Inventory)(x.Cells["extra_1"].Value);
                await CSeller_Inventory.Remove(y.seller_id, y.product_id);
                dataGridView_Inventory.Rows.Remove(x);
            }
        }

        private async void tabControl_MAIN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl_MAIN.SelectedTab.Text.Equals("Inventory"))
            {
                dataGridView_Inventory.Rows.Clear();
                List<CSeller_Inventory> inv_list = await CSeller_Inventory.RetrieveSellerInventoryList(CUser.cur_user.id);
                foreach (CSeller_Inventory x in inv_list)
                {
                    CProduct y = await CProduct.Retrieve(x.product_id);
                    dataGridView_Inventory.Rows.Add
                    (
                        new object[]
                        {
                            x.product_id,
                            y.name,
                            x.price,
                            x.quantity,
                            x.pincode,
                            x.warranty,
                            x,
                            y
                        }
                    );
                }
            }
            else if (tabControl_MAIN.SelectedTab.Text.Equals("Orders"))
            {
                dataGridView_Orders.Rows.Clear();
                List<COrder> order_list = await COrder.RetrieveOrdertBySellerID(CUser.cur_user.id);
                DistanceMatrixRequest req = new DistanceMatrixRequest();
                req.ApiKey = Program.szGoogleMapsAPI_key;
                req.Mode = DistanceMatrixTravelModes.driving;
                foreach (COrder x in order_list)
                {
                    CProduct y = await CProduct.Retrieve(x.product_id);
                    CSeller_Inventory z = await CSeller_Inventory.Retrieve(x.seller_id, x.product_id);
                    CLocation c = await CLocation.Retrieve(x.pincode);
                    String address = x.street + ", " + c.city + ", " + c.state + ", " + c.country + ", " + x.pincode;

                    // Calculate ETA
                    req.Origins = new string[] { z.pincode.ToString() };
                    req.Destinations = new string[] { x.pincode.ToString() };
                    DistanceMatrixResponse resp = await GoogleMapsApi.GoogleMaps.DistanceMatrix.QueryAsync(req);
                    TimeSpan ts = resp.Rows.ElementAt<Row>(0).Elements.ElementAt<Element>(0).Duration.Value;
                    ts = ts - (DateTime.Now - x.started_at.AddDays(1));
                    dataGridView_Orders.Rows.Add
                    (
                        new object[]
                        {
                            x.id,
                            y.name,
                            z.price,
                            x.quantity,
                            address,
                            x.started_at,
                            (ts.Days) + " Day(s) " + (ts.Hours) + " Hour(s)",
                            x.order_status,
                            x,
                            y,
                            z,
                            c
                        }
                    );
                }
            }
        }

        private async void button_Inventory_Save_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow x in dataGridView_Inventory.Rows)
            {
                CSeller_Inventory y = (CSeller_Inventory)(x.Cells["extra_1"].Value);
                await y.Commit(Double.Parse(x.Cells["price"].Value.ToString()),
                               Int32.Parse(x.Cells["quantity"].Value.ToString()),
                               Int32.Parse(x.Cells["pincode"].Value.ToString()),
                               Double.Parse(x.Cells["warranty"].Value.ToString()));
            }
        }

        private void button_Logout_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Dispose();
            new Login().Visible = true;
        }
    }
}
