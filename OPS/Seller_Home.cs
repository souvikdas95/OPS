using System;
using System.Collections;
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
            String text = textBox_Product_Search.Text;
            CCatagory cat_obj = (CCatagory)comboBox_Product_Search.SelectedItem;
            BindingList<CProduct> result = await CProduct.SearchProductBindingList(cat_obj.id, text);
            dataGridView_Product.DataSource = new BindingSource(result, null);
            foreach (DataGridViewColumn x in dataGridView_Product.Columns)
            {
                if (x.Name.Equals("name"))
                {
                    x.DisplayIndex = 0;
                    x.HeaderText = "Name";
                }
                else if (x.Name.Equals("minprice"))
                {
                    x.DisplayIndex = 1;
                    x.HeaderText = "Price";
                }
                else if (x.Name.Equals("sales"))
                {
                    x.DisplayIndex = 2;
                    x.HeaderText = "Sales";
                }
                else if (x.Name.Equals("rating"))
                {
                    x.DisplayIndex = 3;
                    x.HeaderText = "Rating";
                }
                else if (x.Name.Equals("quantity"))
                {
                    x.DisplayIndex = 4;
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
    }
}
