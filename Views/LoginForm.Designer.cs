namespace FLOSSLOOP.Views
{
    partial class LoginForm
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
            panelCard = new Panel();
            buttonLogin = new Button();
            labelPassword = new Label();
            textBoxPassword = new TextBox();
            labelEmail = new Label();
            textBoxEmail = new TextBox();
            panelHeader = new Panel();
            label1 = new Label();
            panelCard.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelCard
            // 
            panelCard.Anchor = AnchorStyles.None;
            panelCard.BackColor = Color.White;
            panelCard.Controls.Add(buttonLogin);
            panelCard.Controls.Add(labelPassword);
            panelCard.Controls.Add(textBoxPassword);
            panelCard.Controls.Add(labelEmail);
            panelCard.Controls.Add(textBoxEmail);
            panelCard.Controls.Add(panelHeader);
            panelCard.Location = new Point(442, 100);
            panelCard.Name = "panelCard";
            panelCard.Size = new Size(380, 413);
            panelCard.TabIndex = 0;
            // 
            // buttonLogin
            // 
            buttonLogin.BackColor = Color.MediumSeaGreen;
            buttonLogin.FlatAppearance.BorderSize = 0;
            buttonLogin.FlatStyle = FlatStyle.Flat;
            buttonLogin.ForeColor = Color.White;
            buttonLogin.Location = new Point(153, 297);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new Size(75, 23);
            buttonLogin.TabIndex = 5;
            buttonLogin.Text = "Login";
            buttonLogin.UseVisualStyleBackColor = false;
            buttonLogin.Click += buttonLogin_Click_1;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(35, 187);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(57, 15);
            labelPassword.TabIndex = 4;
            labelPassword.Text = "Password";
            // 
            // textBoxPassword
            // 
            textBoxPassword.BackColor = Color.LightGray;
            textBoxPassword.Location = new Point(35, 203);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(310, 23);
            textBoxPassword.TabIndex = 3;
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(35, 111);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(36, 15);
            labelEmail.TabIndex = 2;
            labelEmail.Text = "Email";
            // 
            // textBoxEmail
            // 
            textBoxEmail.BackColor = Color.LightGray;
            textBoxEmail.Location = new Point(35, 127);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(310, 23);
            textBoxEmail.TabIndex = 1;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.MediumSeaGreen;
            panelHeader.Controls.Add(label1);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(380, 35);
            panelHeader.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(139, 9);
            label1.Name = "label1";
            label1.Size = new Size(112, 15);
            label1.TabIndex = 0;
            label1.Text = "FLOSSLOOP • Login";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            ClientSize = new Size(1264, 681);
            Controls.Add(panelCard);
            Name = "LoginForm";
            Text = "LoginForm";
            WindowState = FormWindowState.Maximized;
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelCard;
        private Panel panelHeader;
        private Label label1;
        private TextBox textBoxEmail;
        private Label labelPassword;
        private TextBox textBoxPassword;
        private Label labelEmail;
        private Button buttonLogin;
    }
}