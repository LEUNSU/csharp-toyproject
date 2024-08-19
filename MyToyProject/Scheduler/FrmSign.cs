using MetroFramework.Forms;
using Scheduler.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scheduler
{
    public partial class FrmSign : MetroForm
    {
        public FrmSign()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void BtnSign_Click(object sender, EventArgs e)
        {
            string userId = TxtId.Text.Trim();
            string password = TxtPassword.Text.Trim();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("아이디와 비밀번호를 입력하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 데이터베이스 연결
            using (SqlConnection conn = new SqlConnection(Helper.Common.ConnString))
            {
                try
                {
                    conn.Open();

                    // 아이디 중복 체크를 위한 쿼리
                    string checkQuery = @"SELECT COUNT(*) FROM usertbl WHERE userId = @userId";

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@userId", userId);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // 중복된 아이디가 있을 때 메시지 표시
                            MessageBox.Show("이미 있는 아이디입니다.", "중복 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // 중복되지 않으면 삽입 쿼리 실행
                    string query = @"INSERT INTO usertbl (userId, [password])
                             VALUES (@userId, @password)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@password", password);

                        int result = cmd.ExecuteNonQuery(); // 명령을 실행하고 영향받은 행 수를 반환

                        if (result > 0)
                        {
                            MessageBox.Show("회원가입이 완료되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("회원가입에 실패했습니다. 다시 시도하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (Helper.Common.frmLogin != null)
            {
                Helper.Common.frmLogin.Show();
            }
            else
            {
                Helper.Common.frmLogin = new FrmLogin();
                Helper.Common.frmLogin.Show();
            }

            this.Close();
        }
    }
}
