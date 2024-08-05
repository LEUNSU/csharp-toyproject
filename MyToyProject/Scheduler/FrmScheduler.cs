using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace Scheduler
{
    public partial class FrmScheduler : MetroForm
    {
        public FrmScheduler()
        {
            InitializeComponent();
            ScheduleList.CellValueChanged += ScheduleList_CellValueChanged;
            ScheduleList.CurrentCellDirtyStateChanged += ScheduleList_CurrentCellDirtyStateChanged;
            ScheduleList.CellClick += ScheduleList_CellClick; // CellClick �̺�Ʈ �ڵ鷯 �߰�
        }

        private void FrmScheduler_Load(object sender, EventArgs e)
        {
            FrmLogin frm = new FrmLogin();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.TopMost = true;
            frm.ShowDialog();

            if (frm.IsLogin)
            {
                InitializeScheduleList();

                // ���� ��¥�� ���� �� ���� ��¥�� ����
                DateTime today = DateTime.Today;
                InitializeDateControls(today);
                RefreshData(today, today);
            }
            else
            {
                Application.Exit();
            }
        }
        private void InitializeDateControls(DateTime date)
        {
            MonthCalendar.SetDate(date);
            DateTimePicker.Value = date;
            TxtDate.Text = date.ToString("yyyy-MM-dd");
        }

        private void InitializeScheduleList()
        {
            ScheduleList.AutoGenerateColumns = false;
            ScheduleList.Columns.Clear();

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "Id",
                Name = "Id",
                Visible = false
            };
            ScheduleList.Columns.Add(idColumn);

            DataGridViewTextBoxColumn contentColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "����",
                DataPropertyName = "content"
            };
            ScheduleList.Columns.Add(contentColumn);

            DataGridViewTextBoxColumn startDateColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "���� ��¥",
                DataPropertyName = "StartDate"
            };
            ScheduleList.Columns.Add(startDateColumn);

            DataGridViewTextBoxColumn endDateColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "���� ��¥",
                DataPropertyName = "EndDate"
            };
            ScheduleList.Columns.Add(endDateColumn);

            DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "����",
                DataPropertyName = "check",
                Name = "check",
                TrueValue = true,
                FalseValue = false,
                Width = 50
            };
            ScheduleList.Columns.Add(chkColumn);

            // ��ư �÷� �߰�
            DataGridViewButtonColumn memoButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "�޸�",
                Text = "����",
                UseColumnTextForButtonValue = true,
                Name = "MemoButton",
                Width = 80
            };
            ScheduleList.Columns.Add(memoButtonColumn);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSchedule.Text))
            {
                MessageBox.Show("������ �Է��ϼ���.");
                return;
            }

            DateTime startDate = MonthCalendar.SelectionRange.Start;
            DateTime endDate = MonthCalendar.SelectionRange.End;

            if (startDate > endDate) // endDate�� startDate���� �̸� ��� ���� ó��
            {
                MessageBox.Show("�ùٸ� ��¥ ������ �����ϼ���.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(Helper.Common.ConnString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = "INSERT INTO [dbo].[scheduletbl] ([content], [StartDate], [EndDate], [check], [UserId]) VALUES (@content, @StartDate, @EndDate, @check, @UserId)"
                    };

                    cmd.Parameters.AddWithValue("@content", TxtSchedule.Text);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    cmd.Parameters.AddWithValue("@check", GetCheckStateFromGrid());
                    cmd.Parameters.AddWithValue("@UserId", Helper.Common.LoginId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show(this, "���强��!", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TxtSchedule.Clear(); // ���� ���� �� TxtSchedule �ʱ�ȭ
                }

                // ������ ����
                RefreshData(startDate, endDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"���� : {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetCheckStateFromGrid()
        {
            // ������ �׸���信�� üũ ���¸� �������� ����
            // �⺻������ 0�� ��ȯ�ϰų� �ʿ信 ���� ������ �� ��ȯ
            return 0;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (ScheduleList.CurrentRow != null)
            {
                var selectedId = ScheduleList.CurrentRow.Cells["Id"].Value;

                if (selectedId == null || selectedId == DBNull.Value)
                {
                    MessageBox.Show(this, "������ �׸��� �����ϼ���.", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(Helper.Common.ConnString))
                    {
                        conn.Open();

                        // �޸� ���� ���� �߰�
                        var deleteMemoQuery = @"DELETE FROM [dbo].[memo] WHERE [ScheduleId] = @ScheduleId AND [UserId] = @UserId";
                        SqlCommand deleteMemoCmd = new SqlCommand(deleteMemoQuery, conn);
                        deleteMemoCmd.Parameters.AddWithValue("@ScheduleId", selectedId);
                        deleteMemoCmd.Parameters.AddWithValue("@UserId", Helper.Common.LoginId);
                        deleteMemoCmd.ExecuteNonQuery();

                        // ���� ���� ����
                        var deleteScheduleQuery = @"DELETE FROM [dbo].[scheduletbl] WHERE [Id] = @Id AND [UserId] = @UserId";
                        SqlCommand deleteScheduleCmd = new SqlCommand(deleteScheduleQuery, conn);
                        deleteScheduleCmd.Parameters.AddWithValue("@Id", selectedId);
                        deleteScheduleCmd.Parameters.AddWithValue("@UserId", Helper.Common.LoginId);
                        var result = deleteScheduleCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show(this, "���� ����!", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(this, "���� ����!", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // ������ ���� �� �����ͱ׸���並 ���ΰ�Ĩ�ϴ�.
                    DateTime startDate = MonthCalendar.SelectionRange.Start;
                    DateTime endDate = MonthCalendar.SelectionRange.End;
                    RefreshData(startDate, endDate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"���� : {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(this, "������ �׸��� �����ϼ���.", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshData(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Helper.Common.ConnString))
                {
                    conn.Open();

                    var query = @"SELECT [Id], [content], [StartDate], [EndDate], [check] 
                          FROM [dbo].[scheduletbl] 
                          WHERE (([StartDate] BETWEEN @startDate AND @endDate) 
                             OR ([EndDate] BETWEEN @startDate AND @endDate) 
                             OR (@startDate BETWEEN [StartDate] AND [EndDate]) 
                             OR (@endDate BETWEEN [StartDate] AND [EndDate]))
                             AND [UserId] = @UserId";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@startDate", startDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@endDate", endDate);
                    adapter.SelectCommand.Parameters.AddWithValue("@UserId", Helper.Common.LoginId);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "scheduletbl");

                    ScheduleList.DataSource = ds.Tables["scheduletbl"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTimePicker.Value = MonthCalendar.SelectionStart;

            DateTime startDate = MonthCalendar.SelectionRange.Start;
            DateTime endDate = MonthCalendar.SelectionRange.End;

            if (MonthCalendar.SelectionRange.Start == MonthCalendar.SelectionRange.End)
            {
                TxtDate.Text = startDate.ToString("yyyy-MM-dd");
                endDate = startDate; // ���� ��¥�� �� �������� �����Ϸ� ����
            }
            else
            {
                TxtDate.Text = startDate.ToString("yyyy-MM-dd") + "~" + endDate.ToString("yyyy-MM-dd");
            }

            RefreshData(startDate, endDate);
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            MonthCalendar.SetDate(DateTimePicker.Value);
            TxtDate.Text = DateTimePicker.Value.ToString("yyyy-MM-dd");

            RefreshData(DateTimePicker.Value, DateTimePicker.Value);
        }

        private void ScheduleList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && ScheduleList.Columns[e.ColumnIndex].Name == "check")
            {
                var selectedId = ScheduleList.Rows[e.RowIndex].Cells["Id"].Value;
                var isChecked = Convert.ToBoolean(ScheduleList.Rows[e.RowIndex].Cells["check"].Value);

                UpdateCheckStateInDatabase(selectedId, isChecked);
            }
        }

        private void ScheduleList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (ScheduleList.IsCurrentCellDirty)
            {
                ScheduleList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void UpdateCheckStateInDatabase(object id, bool isChecked)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Helper.Common.ConnString))
                {
                    conn.Open();

                    var query = @"UPDATE [dbo].[scheduletbl] 
                          SET [check] = @check 
                          WHERE [Id] = @Id AND [UserId] = @UserId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@check", isChecked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@UserId", Helper.Common.LoginId);

                    var result = cmd.ExecuteNonQuery();

                    if (result == 0)
                    {
                        MessageBox.Show(this, "üũ ���� ������Ʈ ����!", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"���� : {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScheduleList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && ScheduleList.Columns[e.ColumnIndex].Name == "MemoButton")
            {
                var selectedId = ScheduleList.Rows[e.RowIndex].Cells["Id"].Value;
                if (selectedId != null && selectedId != DBNull.Value)
                {
                    // FrmMemo ���� �����ϰ� ID�� ����
                    FrmMemo frmMemo = new FrmMemo(selectedId.ToString());
                    frmMemo.StartPosition = FormStartPosition.CenterParent;
                    frmMemo.ShowDialog(); // ShowDialog�� ��� ��ȭ���ڸ� ǥ���Ͽ� ���� ���� ������ ���
                }
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // �α��� ���� �ʱ�ȭ
            Helper.Common.LoginId = string.Empty;

            // ������ �׸���� �ʱ�ȭ
            ScheduleList.DataSource = null;

            // ���� â�� ����ϴ�.
            this.Hide();

            // �α��� ���� �� �ν��Ͻ��� ����
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.StartPosition = FormStartPosition.CenterScreen;
            frmLogin.TopMost = true;

            // �α��� ���� ��� ��ȭ���ڷ� ǥ��
            frmLogin.ShowDialog();

            // �α��� ���� ���� �� �α��� ���� ���θ� Ȯ��
            if (frmLogin.IsLogin)
            {
                // ����ڰ� �α��ο� ������ ���, ������ ���� ���� �ٽ� ǥ��
                this.Show();

                // ���� ��¥�� �������� �ʱ�ȭ
                DateTime today = DateTime.Today;
                InitializeScheduleList();
                InitializeDateControls(today);
                RefreshData(today, today);
            }
            else
            {
                // �α��� ���г� ����ڰ� ����� ���, ���ø����̼��� ����
                Application.Exit();
            }
        }
    }
}