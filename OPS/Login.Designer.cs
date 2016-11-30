namespace OPS
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel_Details = new System.Windows.Forms.TableLayoutPanel();
            this.textBox_Username = new System.Windows.Forms.TextBox();
            this.label_Account = new System.Windows.Forms.Label();
            this.comboBox_Account = new System.Windows.Forms.ComboBox();
            this.label_Password = new System.Windows.Forms.Label();
            this.textBox_Password = new System.Windows.Forms.TextBox();
            this.label_Username = new System.Windows.Forms.Label();
            this.tableLayoutPanel_Buttons = new System.Windows.Forms.TableLayoutPanel();
            this.button_Submit = new System.Windows.Forms.Button();
            this.button_Register = new System.Windows.Forms.Button();
            this.label_Login = new System.Windows.Forms.Label();
            this.tableLayoutPanel_MAIN = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel_Details.SuspendLayout();
            this.tableLayoutPanel_Buttons.SuspendLayout();
            this.tableLayoutPanel_MAIN.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_Details
            // 
            this.tableLayoutPanel_Details.ColumnCount = 4;
            this.tableLayoutPanel_Details.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Details.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel_Details.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel_Details.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel_Details.Controls.Add(this.textBox_Username, 2, 1);
            this.tableLayoutPanel_Details.Controls.Add(this.label_Account, 1, 5);
            this.tableLayoutPanel_Details.Controls.Add(this.comboBox_Account, 2, 5);
            this.tableLayoutPanel_Details.Controls.Add(this.label_Password, 1, 3);
            this.tableLayoutPanel_Details.Controls.Add(this.textBox_Password, 2, 3);
            this.tableLayoutPanel_Details.Controls.Add(this.label_Username, 1, 1);
            this.tableLayoutPanel_Details.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Details.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel_Details.Name = "tableLayoutPanel_Details";
            this.tableLayoutPanel_Details.RowCount = 7;
            this.tableLayoutPanel_Details.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel_Details.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_Details.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel_Details.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_Details.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel_Details.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_Details.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel_Details.Size = new System.Drawing.Size(278, 90);
            this.tableLayoutPanel_Details.TabIndex = 2;
            // 
            // textBox_Username
            // 
            this.textBox_Username.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Username.Location = new System.Drawing.Point(117, 11);
            this.textBox_Username.Name = "textBox_Username";
            this.textBox_Username.Size = new System.Drawing.Size(135, 20);
            this.textBox_Username.TabIndex = 3;
            // 
            // label_Account
            // 
            this.label_Account.AutoSize = true;
            this.label_Account.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Account.Location = new System.Drawing.Point(23, 60);
            this.label_Account.Name = "label_Account";
            this.label_Account.Size = new System.Drawing.Size(56, 16);
            this.label_Account.TabIndex = 2;
            this.label_Account.Text = "Account";
            // 
            // comboBox_Account
            // 
            this.comboBox_Account.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_Account.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Account.FormattingEnabled = true;
            this.comboBox_Account.Items.AddRange(new object[] {
            "Customer",
            "Seller"});
            this.comboBox_Account.Location = new System.Drawing.Point(117, 63);
            this.comboBox_Account.Name = "comboBox_Account";
            this.comboBox_Account.Size = new System.Drawing.Size(135, 21);
            this.comboBox_Account.TabIndex = 5;
            // 
            // label_Password
            // 
            this.label_Password.AutoSize = true;
            this.label_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Password.Location = new System.Drawing.Point(23, 34);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(68, 16);
            this.label_Password.TabIndex = 1;
            this.label_Password.Text = "Password";
            // 
            // textBox_Password
            // 
            this.textBox_Password.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Password.Location = new System.Drawing.Point(117, 37);
            this.textBox_Password.Name = "textBox_Password";
            this.textBox_Password.PasswordChar = '*';
            this.textBox_Password.Size = new System.Drawing.Size(135, 20);
            this.textBox_Password.TabIndex = 4;
            // 
            // label_Username
            // 
            this.label_Username.AutoSize = true;
            this.label_Username.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Username.Location = new System.Drawing.Point(23, 8);
            this.label_Username.Name = "label_Username";
            this.label_Username.Size = new System.Drawing.Size(71, 16);
            this.label_Username.TabIndex = 0;
            this.label_Username.Text = "Username";
            // 
            // tableLayoutPanel_Buttons
            // 
            this.tableLayoutPanel_Buttons.ColumnCount = 5;
            this.tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Buttons.Controls.Add(this.button_Submit, 3, 0);
            this.tableLayoutPanel_Buttons.Controls.Add(this.button_Register, 1, 0);
            this.tableLayoutPanel_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_Buttons.Location = new System.Drawing.Point(3, 124);
            this.tableLayoutPanel_Buttons.Name = "tableLayoutPanel_Buttons";
            this.tableLayoutPanel_Buttons.RowCount = 1;
            this.tableLayoutPanel_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Buttons.Size = new System.Drawing.Size(278, 34);
            this.tableLayoutPanel_Buttons.TabIndex = 3;
            // 
            // button_Submit
            // 
            this.button_Submit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_Submit.AutoSize = true;
            this.button_Submit.Location = new System.Drawing.Point(161, 5);
            this.button_Submit.Name = "button_Submit";
            this.button_Submit.Size = new System.Drawing.Size(49, 23);
            this.button_Submit.TabIndex = 1;
            this.button_Submit.Text = "Submit";
            this.button_Submit.UseVisualStyleBackColor = true;
            this.button_Submit.Click += new System.EventHandler(this.button_Submit_Click);
            // 
            // button_Register
            // 
            this.button_Register.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Register.Location = new System.Drawing.Point(68, 5);
            this.button_Register.Name = "button_Register";
            this.button_Register.Size = new System.Drawing.Size(67, 23);
            this.button_Register.TabIndex = 2;
            this.button_Register.Text = "Register";
            this.button_Register.UseVisualStyleBackColor = true;
            this.button_Register.Click += new System.EventHandler(this.button_Register_Click);
            // 
            // label_Login
            // 
            this.label_Login.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_Login.AutoSize = true;
            this.label_Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Login.Location = new System.Drawing.Point(107, 0);
            this.label_Login.Name = "label_Login";
            this.label_Login.Size = new System.Drawing.Size(70, 25);
            this.label_Login.TabIndex = 0;
            this.label_Login.Text = "Login";
            this.label_Login.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tableLayoutPanel_MAIN
            // 
            this.tableLayoutPanel_MAIN.ColumnCount = 1;
            this.tableLayoutPanel_MAIN.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_MAIN.Controls.Add(this.label_Login, 0, 0);
            this.tableLayoutPanel_MAIN.Controls.Add(this.tableLayoutPanel_Details, 0, 1);
            this.tableLayoutPanel_MAIN.Controls.Add(this.tableLayoutPanel_Buttons, 0, 2);
            this.tableLayoutPanel_MAIN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_MAIN.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel_MAIN.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_MAIN.Name = "tableLayoutPanel_MAIN";
            this.tableLayoutPanel_MAIN.RowCount = 3;
            this.tableLayoutPanel_MAIN.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_MAIN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_MAIN.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_MAIN.Size = new System.Drawing.Size(284, 161);
            this.tableLayoutPanel_MAIN.TabIndex = 4;
            // 
            // Login
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Controls.Add(this.tableLayoutPanel_MAIN);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "Login";
            this.Text = "OPS : Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.tableLayoutPanel_Details.ResumeLayout(false);
            this.tableLayoutPanel_Details.PerformLayout();
            this.tableLayoutPanel_Buttons.ResumeLayout(false);
            this.tableLayoutPanel_Buttons.PerformLayout();
            this.tableLayoutPanel_MAIN.ResumeLayout(false);
            this.tableLayoutPanel_MAIN.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_Submit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Details;
        private System.Windows.Forms.Label label_Username;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.Label label_Account;
        private System.Windows.Forms.TextBox textBox_Username;
        private System.Windows.Forms.TextBox textBox_Password;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Buttons;
        private System.Windows.Forms.ComboBox comboBox_Account;
        private System.Windows.Forms.Label label_Login;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_MAIN;
        private System.Windows.Forms.Button button_Register;
    }
}