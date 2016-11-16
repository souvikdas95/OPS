using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OPS.Program;
using MySql.Data.MySqlClient;

namespace OPS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            comboBox_Account.SelectedIndex = 0;

            ConnectSQL();
        }

        private void button_Register_Click(object sender, EventArgs e)
        {
            new Register(this).Visible = true;
            this.Visible = false;
        }

        private async void button_Submit_Click(object sender, EventArgs e)
        {
            // Get & Validate Username
            String username = textBox_Username.Text.Trim();
            if (!CUtils.IsValidUsername(username))
            {
                if (CUtils.LastLogMsg != null)
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Cause: Invalid Username!", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Get & Validate Password
            String password = textBox_Password.Text.Trim();
            if (!CUtils.IsValidPassword(password))
            {
                if (CUtils.LastLogMsg != null)
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Cause: Invalid Password!", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            password = CUtils.sha256(password);

            // Get & Validate Account/User Type
            Byte type = (Byte)comboBox_Account.SelectedIndex;
            if (!(await CUtils.IsValidUserType(type)))
            {
                if (CUtils.LastLogMsg != null)
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Cause: Invalid Account Type!", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Login User
            CUser.cur_user = await CUser.Retrieve(username, password, type);
            if (CUser.cur_user == null)
            {
                if (CUtils.LastLogMsg != null)
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            /*if (!(await CUser.cur_user.login()))
            {
                if (CUtils.LastLogMsg != null)
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }*/
            MessageBox.Show("You have Successfully Logged In!" + type, "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            switch(type)
            {
                case 0: new Customer_Home(this).Visible = true; break;
                case 1: new Seller_Home(this).Visible = true; break;
                //case 2: new OrderManager_Home(this).Visible = true; break;
                //case 3: new ProductManager_Home(this).Visible = true; break;
            }
            this.Visible = false;
        }
    }
}
