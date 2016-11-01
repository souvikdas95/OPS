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
    public partial class Register : Form
    {
        private Form prev;

        public Register(Form frame)
        {
            prev = frame;
            this.Location = prev.Location;

            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            comboBox_Account.SelectedIndex = 0;
        }

        private void Register_FormClosed(object sender, FormClosedEventArgs e)
        {
            prev.StartPosition = this.StartPosition;
            prev.Location = this.Location;
            prev.Visible = true;
        }

        private void button_Back_Click(object sender, EventArgs e)
        {
            prev.StartPosition = this.StartPosition;
            prev.Location = this.Location;
            prev.Visible = true;
            this.Close();
            this.Dispose();
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

            // Get & Validate EmailID
            String email = textBox_EmailID.Text.Trim();
            if (!CUtils.IsValidEmail(email))
            {
                if (CUtils.LastLogMsg != null)
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Cause: Invalid Email!", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Register User
            if (!(await CUser.Register(username, password, email, type)))
            {
                if (CUtils.LastLogMsg != null)
                    MessageBox.Show("Cause: " + CUtils.LastLogMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Cause: Unknown", "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            MessageBox.Show("You have Successfully Registered!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            prev.StartPosition = this.StartPosition;
            prev.Location = this.Location;
            prev.Visible = true;
            this.Close();
            this.Dispose();
        }
    }
}
