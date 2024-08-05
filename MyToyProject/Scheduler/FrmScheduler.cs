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
            ScheduleList.CellClick += ScheduleList_CellClick; // CellClick 이벤트 핸들러 추가
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

                // 현재 날짜를 시작 및 종료 날짜로 설정
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
                HeaderText = "내용",
                DataPropertyName = "content"
            };
            ScheduleList.Columns.Add(contentColumn);

            DataGridViewTextBoxColumn startDateColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "시작 날짜",
                DataPropertyName = "StartDate"
            };
            ScheduleList.Columns.Add(startDateColumn);

            DataGridViewTextBoxColumn endDateColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "종료 날짜",
                DataPropertyName = "EndDate"
            };
            ScheduleList.Columns.Add(endDateColumn);

            DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "실행",
                DataPropertyName = "check",
                Name = "check",
                TrueValue = true,
                FalseValue = false,
                Width = 50
            };
            ScheduleList.Columns.Add(chkColumn);

            // 버튼 컬럼 추가
            DataGridViewButtonColumn memoButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "메모",
                Text = "열기",
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
                MessageBox.Show("일정을 입력하세요.");
                return;
            }

            DateTime startDate = MonthCalendar.SelectionRange.Start;
            DateTime endDate = MonthCalendar.SelectionRange.End;

            if (startDate > endDate) // endDate가 startDate보다 이른 경우 예외 처리
            {
                MessageBox.Show("올바른 날짜 범위를 선택하세요.");
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

                    MessageBox.Show(this, "저장성공!", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TxtSchedule.Clear(); // 저장 성공 시 TxtSchedule 초기화
                }

                // 데이터 갱신
                RefreshData(startDate, endDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 : {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetCheckStateFromGrid()
        {
            // 데이터 그리드뷰에서 체크 상태를 가져오는 로직
            // 기본적으로 0을 반환하거나 필요에 따라 적절한 값 반환
            return 0;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (ScheduleList.CurrentRow != null)
            {
                var selectedId = ScheduleList.CurrentRow.Cells["Id"].Value;

                if (selectedId == null || selectedId == DBNull.Value)
                {
                    MessageBox.Show(this, "삭제할 항목을 선택하세요.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(Helper.Common.ConnString))
                    {
                        conn.Open();

                        // 메모 삭제 쿼리 추가
                        var deleteMemoQuery = @"DELETE FROM [dbo].[memo] WHERE [ScheduleId] = @ScheduleId AND [UserId] = @UserId";
                        SqlCommand deleteMemoCmd = new SqlCommand(deleteMemoQuery, conn);
                        deleteMemoCmd.Parameters.AddWithValue("@ScheduleId", selectedId);
                        deleteMemoCmd.Parameters.AddWithValue("@UserId", Helper.Common.LoginId);
                        deleteMemoCmd.ExecuteNonQuery();

                        // 일정 삭제 쿼리
                        var deleteScheduleQuery = @"DELETE FROM [dbo].[scheduletbl] WHERE [Id] = @Id AND [UserId] = @UserId";
                        SqlCommand deleteScheduleCmd = new SqlCommand(deleteScheduleQuery, conn);
                        deleteScheduleCmd.Parameters.AddWithValue("@Id", selectedId);
                        deleteScheduleCmd.Parameters.AddWithValue("@UserId", Helper.Common.LoginId);
                        var result = deleteScheduleCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show(this, "삭제 성공!", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(this, "삭제 실패!", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // 데이터 삭제 후 데이터그리드뷰를 새로고칩니다.
                    DateTime startDate = MonthCalendar.SelectionRange.Start;
                    DateTime endDate = MonthCalendar.SelectionRange.End;
                    RefreshData(startDate, endDate);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, $"오류 : {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(this, "삭제할 항목을 선택하세요.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                endDate = startDate; // 단일 날짜일 때 종료일을 시작일로 설정
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
                        MessageBox.Show(this, "체크 상태 업데이트 실패!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 : {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScheduleList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && ScheduleList.Columns[e.ColumnIndex].Name == "MemoButton")
            {
                var selectedId = ScheduleList.Rows[e.RowIndex].Cells["Id"].Value;
                if (selectedId != null && selectedId != DBNull.Value)
                {
                    // FrmMemo 폼을 생성하고 ID를 전달
                    FrmMemo frmMemo = new FrmMemo(selectedId.ToString());
                    frmMemo.StartPosition = FormStartPosition.CenterParent;
                    frmMemo.ShowDialog(); // ShowDialog는 모달 대화상자를 표시하여 폼이 닫힐 때까지 대기
                }
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // 로그인 상태 초기화
            Helper.Common.LoginId = string.Empty;

            // 데이터 그리드뷰 초기화
            ScheduleList.DataSource = null;

            // 현재 창을 숨깁니다.
            this.Hide();

            // 로그인 폼의 새 인스턴스를 생성
            FrmLogin frmLogin = new FrmLogin();
            frmLogin.StartPosition = FormStartPosition.CenterScreen;
            frmLogin.TopMost = true;

            // 로그인 폼을 모달 대화상자로 표시
            frmLogin.ShowDialog();

            // 로그인 폼이 닫힌 후 로그인 성공 여부를 확인
            if (frmLogin.IsLogin)
            {
                // 사용자가 로그인에 성공한 경우, 숨겨진 현재 폼을 다시 표시
                this.Show();

                // 오늘 날짜를 기준으로 초기화
                DateTime today = DateTime.Today;
                InitializeScheduleList();
                InitializeDateControls(today);
                RefreshData(today, today);
            }
            else
            {
                // 로그인 실패나 사용자가 취소한 경우, 애플리케이션을 종료
                Application.Exit();
            }
        }
    }
}