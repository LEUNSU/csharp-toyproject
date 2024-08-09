namespace Scheduler
{
    partial class FrmSign
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
            TxtId = new TextBox();
            TxtPassword = new TextBox();
            label1 = new Label();
            label2 = new Label();
            BtnSign = new Button();
            BtnCancel = new Button();
            SuspendLayout();
            // 
            // TxtId
            // 
            TxtId.Location = new Point(111, 120);
            TxtId.Name = "TxtId";
            TxtId.Size = new Size(100, 23);
            TxtId.TabIndex = 0;
            // 
            // TxtPassword
            // 
            TxtPassword.Location = new Point(111, 172);
            TxtPassword.Name = "TxtPassword";
            TxtPassword.PasswordChar = '●';
            TxtPassword.Size = new Size(100, 23);
            TxtPassword.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(86, 128);
            label1.Name = "label1";
            label1.Size = new Size(19, 15);
            label1.TabIndex = 2;
            label1.Text = "ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(48, 175);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 3;
            label2.Text = "password";
            // 
            // BtnSign
            // 
            BtnSign.Location = new Point(57, 252);
            BtnSign.Name = "BtnSign";
            BtnSign.Size = new Size(75, 27);
            BtnSign.TabIndex = 5;
            BtnSign.Text = "등록";
            BtnSign.UseVisualStyleBackColor = true;
            BtnSign.Click += BtnSign_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(150, 252);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 27);
            BtnCancel.TabIndex = 6;
            BtnCancel.Text = "취소";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // FrmSign
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 344);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSign);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(TxtPassword);
            Controls.Add(TxtId);
            Name = "FrmSign";
            Text = "Sign Up";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TxtId;
        private TextBox TxtPassword;
        private Label label1;
        private Label label2;
        private Button BtnSign;
        private Button BtnCancel;
    }
}