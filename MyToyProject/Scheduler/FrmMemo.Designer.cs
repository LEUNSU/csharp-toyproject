namespace Scheduler
{
    partial class FrmMemo
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
            TxtMemo = new TextBox();
            BtnSave = new Button();
            BtnCancel = new Button();
            SuspendLayout();
            // 
            // TxtMemo
            // 
            TxtMemo.Location = new Point(23, 53);
            TxtMemo.Multiline = true;
            TxtMemo.Name = "TxtMemo";
            TxtMemo.Size = new Size(256, 222);
            TxtMemo.TabIndex = 0;
            // 
            // BtnSave
            // 
            BtnSave.Location = new Point(70, 292);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(75, 27);
            BtnSave.TabIndex = 1;
            BtnSave.Text = "저장";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(151, 292);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 27);
            BtnCancel.TabIndex = 2;
            BtnCancel.Text = "취소";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // FrmMemo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(302, 334);
            Controls.Add(BtnCancel);
            Controls.Add(BtnSave);
            Controls.Add(TxtMemo);
            Name = "FrmMemo";
            Text = "Memo";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TxtMemo;
        private Button BtnSave;
        private Button BtnCancel;
    }
}