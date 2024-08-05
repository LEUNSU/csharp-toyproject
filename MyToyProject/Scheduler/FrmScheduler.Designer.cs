namespace Scheduler
{
    partial class FrmScheduler
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            MonthCalendar = new MonthCalendar();
            DateTimePicker = new DateTimePicker();
            TxtDate = new TextBox();
            BtnSave = new Button();
            BtnDelete = new Button();
            TxtSchedule = new TextBox();
            ScheduleList = new DataGridView();
            Schedule = new GroupBox();
            BtnLogout = new Button();
            ((System.ComponentModel.ISupportInitialize)ScheduleList).BeginInit();
            Schedule.SuspendLayout();
            SuspendLayout();
            // 
            // MonthCalendar
            // 
            MonthCalendar.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point, 129);
            MonthCalendar.Location = new Point(12, 74);
            MonthCalendar.Name = "MonthCalendar";
            MonthCalendar.TabIndex = 0;
            MonthCalendar.DateChanged += MonthCalendar_DateChanged;
            // 
            // DateTimePicker
            // 
            DateTimePicker.Location = new Point(12, 33);
            DateTimePicker.Name = "DateTimePicker";
            DateTimePicker.Size = new Size(220, 23);
            DateTimePicker.TabIndex = 1;
            // 
            // TxtDate
            // 
            TxtDate.BackColor = Color.FromArgb(224, 224, 224);
            TxtDate.Location = new Point(244, 33);
            TxtDate.Name = "TxtDate";
            TxtDate.Size = new Size(430, 23);
            TxtDate.TabIndex = 2;
            // 
            // BtnSave
            // 
            BtnSave.Location = new Point(506, 277);
            BtnSave.Name = "BtnSave";
            BtnSave.Size = new Size(81, 28);
            BtnSave.TabIndex = 4;
            BtnSave.Text = "저장";
            BtnSave.UseVisualStyleBackColor = true;
            BtnSave.Click += BtnSave_Click;
            // 
            // BtnDelete
            // 
            BtnDelete.Location = new Point(593, 277);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(81, 28);
            BtnDelete.TabIndex = 5;
            BtnDelete.Text = "삭제";
            BtnDelete.UseVisualStyleBackColor = true;
            BtnDelete.Click += BtnDelete_Click;
            // 
            // TxtSchedule
            // 
            TxtSchedule.Location = new Point(244, 248);
            TxtSchedule.Name = "TxtSchedule";
            TxtSchedule.Size = new Size(432, 23);
            TxtSchedule.TabIndex = 6;
            // 
            // ScheduleList
            // 
            ScheduleList.AllowUserToAddRows = false;
            ScheduleList.AllowUserToDeleteRows = false;
            ScheduleList.BackgroundColor = SystemColors.ControlLight;
            ScheduleList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ScheduleList.Location = new Point(244, 68);
            ScheduleList.Name = "ScheduleList";
            ScheduleList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ScheduleList.Size = new Size(430, 174);
            ScheduleList.TabIndex = 7;
            // 
            // Schedule
            // 
            Schedule.Controls.Add(ScheduleList);
            Schedule.Controls.Add(TxtSchedule);
            Schedule.Controls.Add(BtnDelete);
            Schedule.Controls.Add(BtnSave);
            Schedule.Controls.Add(TxtDate);
            Schedule.Controls.Add(DateTimePicker);
            Schedule.Controls.Add(MonthCalendar);
            Schedule.Location = new Point(12, 56);
            Schedule.Name = "Schedule";
            Schedule.Size = new Size(687, 321);
            Schedule.TabIndex = 0;
            Schedule.TabStop = false;
            // 
            // BtnLogout
            // 
            BtnLogout.BackgroundImage = Properties.Resources.free_icon_user_logout_17123238;
            BtnLogout.BackgroundImageLayout = ImageLayout.Stretch;
            BtnLogout.Location = new Point(665, 27);
            BtnLogout.Name = "BtnLogout";
            BtnLogout.Size = new Size(34, 32);
            BtnLogout.TabIndex = 1;
            BtnLogout.UseVisualStyleBackColor = true;
            BtnLogout.Click += BtnLogout_Click;
            // 
            // FrmScheduler
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(711, 400);
            Controls.Add(BtnLogout);
            Controls.Add(Schedule);
            Name = "FrmScheduler";
            Text = "My Schedule";
            Load += FrmScheduler_Load;
            ((System.ComponentModel.ISupportInitialize)ScheduleList).EndInit();
            Schedule.ResumeLayout(false);
            Schedule.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private MonthCalendar MonthCalendar;
        private DateTimePicker DateTimePicker;
        private TextBox TxtDate;
        private Button BtnSave;
        private Button BtnDelete;
        private TextBox TxtSchedule;
        private DataGridView ScheduleList;
        private GroupBox Schedule;
        private Button BtnLogout;
    }
}
