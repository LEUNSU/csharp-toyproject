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
            MemoSave = new Button();
            MemoCancel = new Button();
            lstMemoList = new ListBox();
            SuspendLayout();
            // 
            // TxtMemo
            // 
            TxtMemo.Location = new Point(23, 256);
            TxtMemo.Multiline = true;
            TxtMemo.Name = "TxtMemo";
            TxtMemo.Size = new Size(256, 30);
            TxtMemo.TabIndex = 0;
            // 
            // MemoSave
            // 
            MemoSave.Location = new Point(70, 292);
            MemoSave.Name = "MemoSave";
            MemoSave.Size = new Size(75, 27);
            MemoSave.TabIndex = 1;
            MemoSave.Text = "저장";
            MemoSave.UseVisualStyleBackColor = true;
            MemoSave.Click += MemoSave_Click;
            // 
            // MemoCancel
            // 
            MemoCancel.Location = new Point(151, 292);
            MemoCancel.Name = "MemoCancel";
            MemoCancel.Size = new Size(75, 27);
            MemoCancel.TabIndex = 2;
            MemoCancel.Text = "취소";
            MemoCancel.UseVisualStyleBackColor = true;
            MemoCancel.Click += MemoCancel_Click;
            // 
            // lstMemoList
            // 
            lstMemoList.FormattingEnabled = true;
            lstMemoList.ItemHeight = 15;
            lstMemoList.Location = new Point(25, 63);
            lstMemoList.Name = "lstMemoList";
            lstMemoList.Size = new Size(254, 184);
            lstMemoList.TabIndex = 3;
            // 
            // FrmMemo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(302, 334);
            Controls.Add(lstMemoList);
            Controls.Add(MemoCancel);
            Controls.Add(MemoSave);
            Controls.Add(TxtMemo);
            Name = "FrmMemo";
            Text = "Memo";
            TransparencyKey = Color.Ivory;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TxtMemo;
        private Button MemoSave;
        private Button MemoCancel;
        private ListBox lstMemoList;
    }
}