using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace Scheduler
{
    public partial class FrmMemo : MetroForm
    {
        private string _scheduleId;

        public FrmMemo(string scheduleId)
        {
            InitializeComponent();
            _scheduleId = scheduleId;
            this.Load += new System.EventHandler(this.FrmMemo_Load);
        }

        private void FrmMemo_Load(object sender, EventArgs e)
        {
            LoadMemo();      // 기존 메모 내용 로드
            LoadMemoList();  // 메모 목록 로드
        }

        private void LoadMemo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Helper.Common.ConnString))
                {
                    conn.Open();
                    var query = "SELECT [Content] FROM [dbo].[memo] WHERE [ScheduleId] = @ScheduleId AND [UserId] = @UserId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ScheduleId", _scheduleId);
                        cmd.Parameters.AddWithValue("@UserId", Helper.Common.LoginId);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            TxtMemo.Text = result.ToString();
                        }
                        else
                        {
                            TxtMemo.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 : {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMemoList()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Helper.Common.ConnString))
                {
                    conn.Open();
                    var query = "SELECT [Content] FROM [dbo].[memo] WHERE [ScheduleId] = @ScheduleId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ScheduleId", _scheduleId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            lstMemoList.Items.Clear(); // 기존 항목을 지움
                            while (reader.Read())
                            {
                                lstMemoList.Items.Add(reader["Content"].ToString()); // ListBox에 메모 내용을 추가
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 : {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveMemo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Helper.Common.ConnString))
                {
                    conn.Open();

                    var query = @"IF EXISTS (SELECT 1 FROM [dbo].[memo] WHERE [ScheduleId] = @ScheduleId AND [UserId] = @UserId)
                          BEGIN
                              UPDATE [dbo].[memo] SET [Content] = @Content WHERE [ScheduleId] = @ScheduleId AND [UserId] = @UserId
                          END
                          ELSE
                          BEGIN
                              INSERT INTO [dbo].[memo] ([ScheduleId], [Content], [UserId]) VALUES (@ScheduleId, @Content, @UserId)
                          END";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ScheduleId", _scheduleId);
                        cmd.Parameters.AddWithValue("@Content", TxtMemo.Text);
                        cmd.Parameters.AddWithValue("@UserId", Helper.Common.LoginId);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(this, "메모 저장 성공!", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"오류 : {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MemoSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtMemo.Text))
            {
                MessageBox.Show("메모를 입력하세요.");
                return;
            }

            else SaveMemo();
        }

        private void MemoCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}