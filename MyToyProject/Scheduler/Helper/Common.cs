using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Helper
{
    internal class Common
    {
        // 정적으로 만드는 공통 연결 문자열
        public static readonly string ConnString = "Data Source=localhost;" +
                                          "Initial Catalog=MyScheduler;" +
                                          "Persist Security Info=True;" +
                                          "User ID=sa;Encrypt=False;Password=mssql_p@ss";

        // 로그인아이디
        public static string LoginId { get; set; }

        public static FrmSign StatSignFrm;
        public static FrmLogin StatLoginFrm;
    }
}
