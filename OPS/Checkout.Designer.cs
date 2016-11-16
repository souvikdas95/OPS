namespace OPS
{
    partial class Checkout
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
            this.label_Street = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.richTextBox_Street = new System.Windows.Forms.RichTextBox();
            this.label_Pincode = new System.Windows.Forms.Label();
            this.comboBox_Pincode = new System.Windows.Forms.ComboBox();
            this.label_Contact = new System.Windows.Forms.Label();
            this.label_DebitCard = new System.Windows.Forms.Label();
            this.textBox_DebitCard = new System.Windows.Forms.TextBox();
            this.textBox_Cart_GrandTotal = new System.Windows.Forms.TextBox();
            this.button_Pay = new System.Windows.Forms.Button();
            this.textBox_Contact = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_Street
            // 
            this.label_Street.AutoSize = true;
            this.label_Street.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Street.Location = new System.Drawing.Point(3, 0);
            this.label_Street.Name = "label_Street";
            this.label_Street.Size = new System.Drawing.Size(43, 16);
            this.label_Street.TabIndex = 0;
            this.label_Street.Text = "Street";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label_Street, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox_Street, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_Pincode, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_Pincode, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_Contact, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_DebitCard, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBox_DebitCard, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBox_Cart_GrandTotal, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.button_Pay, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBox_Contact, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(259, 236);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // richTextBox_Street
            // 
            this.richTextBox_Street.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Street.Location = new System.Drawing.Point(81, 3);
            this.richTextBox_Street.Name = "richTextBox_Street";
            this.richTextBox_Street.Size = new System.Drawing.Size(175, 122);
            this.richTextBox_Street.TabIndex = 3;
            this.richTextBox_Street.Text = "";
            // 
            // label_Pincode
            // 
            this.label_Pincode.AutoSize = true;
            this.label_Pincode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Pincode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Pincode.Location = new System.Drawing.Point(3, 128);
            this.label_Pincode.Name = "label_Pincode";
            this.label_Pincode.Size = new System.Drawing.Size(72, 27);
            this.label_Pincode.TabIndex = 1;
            this.label_Pincode.Text = "Pincode";
            // 
            // comboBox_Pincode
            // 
            this.comboBox_Pincode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox_Pincode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox_Pincode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_Pincode.FormattingEnabled = true;
            this.comboBox_Pincode.Location = new System.Drawing.Point(81, 131);
            this.comboBox_Pincode.Name = "comboBox_Pincode";
            this.comboBox_Pincode.Size = new System.Drawing.Size(175, 21);
            this.comboBox_Pincode.TabIndex = 9;
            // 
            // label_Contact
            // 
            this.label_Contact.AutoSize = true;
            this.label_Contact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Contact.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Contact.Location = new System.Drawing.Point(3, 155);
            this.label_Contact.Name = "label_Contact";
            this.label_Contact.Size = new System.Drawing.Size(72, 26);
            this.label_Contact.TabIndex = 13;
            this.label_Contact.Text = "Contact";
            // 
            // label_DebitCard
            // 
            this.label_DebitCard.AutoSize = true;
            this.label_DebitCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_DebitCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_DebitCard.Location = new System.Drawing.Point(3, 181);
            this.label_DebitCard.Name = "label_DebitCard";
            this.label_DebitCard.Size = new System.Drawing.Size(72, 26);
            this.label_DebitCard.TabIndex = 2;
            this.label_DebitCard.Text = "Debit Card";
            // 
            // textBox_DebitCard
            // 
            this.textBox_DebitCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_DebitCard.Location = new System.Drawing.Point(81, 184);
            this.textBox_DebitCard.Name = "textBox_DebitCard";
            this.textBox_DebitCard.Size = new System.Drawing.Size(175, 20);
            this.textBox_DebitCard.TabIndex = 10;
            // 
            // textBox_Cart_GrandTotal
            // 
            this.textBox_Cart_GrandTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Cart_GrandTotal.Enabled = false;
            this.textBox_Cart_GrandTotal.Location = new System.Drawing.Point(3, 210);
            this.textBox_Cart_GrandTotal.Name = "textBox_Cart_GrandTotal";
            this.textBox_Cart_GrandTotal.Size = new System.Drawing.Size(72, 20);
            this.textBox_Cart_GrandTotal.TabIndex = 12;
            // 
            // button_Pay
            // 
            this.button_Pay.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_Pay.Location = new System.Drawing.Point(181, 210);
            this.button_Pay.Name = "button_Pay";
            this.button_Pay.Size = new System.Drawing.Size(75, 23);
            this.button_Pay.TabIndex = 14;
            this.button_Pay.Text = "Pay";
            this.button_Pay.UseVisualStyleBackColor = true;
            this.button_Pay.Click += new System.EventHandler(this.button_Pay_Click);
            // 
            // textBox_Contact
            // 
            this.textBox_Contact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_Contact.Location = new System.Drawing.Point(81, 158);
            this.textBox_Contact.Name = "textBox_Contact";
            this.textBox_Contact.Size = new System.Drawing.Size(175, 20);
            this.textBox_Contact.TabIndex = 15;
            // 
            // Checkout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 236);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(275, 275);
            this.MinimumSize = new System.Drawing.Size(275, 275);
            this.Name = "Checkout";
            this.Text = "Checkout";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Checkout_FormClosed);
            this.Load += new System.EventHandler(this.Checkout_Load);
            this.Click += new System.EventHandler(this.button_Pay_Click);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_Street;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_Pincode;
        private System.Windows.Forms.Label label_DebitCard;
        private System.Windows.Forms.RichTextBox richTextBox_Street;
        private System.Windows.Forms.ComboBox comboBox_Pincode;
        private System.Windows.Forms.TextBox textBox_DebitCard;
        private System.Windows.Forms.TextBox textBox_Cart_GrandTotal;
        private System.Windows.Forms.Label label_Contact;
        private System.Windows.Forms.Button button_Pay;
        private System.Windows.Forms.TextBox textBox_Contact;
    }
}