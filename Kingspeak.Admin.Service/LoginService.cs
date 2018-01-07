using Kingspeak.Admin.Models;
using Kingsun.Core.Utils;
using Kingsun.Framework.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingspeak.Admin.Service
{
    public class LoginService : SqlSugarManager
    {
        public KingResponse AdminLogin(string userName, string passWord)
        {
            passWord = StringHelper.GetMD5(passWord);
            Tb_Admin_UserInfo userinfo = Get<Tb_Admin_UserInfo>(it => it.UserName == userName && it.PassWord == passWord);
            if (userinfo != null)
            {
                Task.Run(() => UpdateLastLoginDate(userinfo.UserID.Value));
                return KingResponse.GetResponse(userinfo);
            }
            else
            {
                return KingResponse.GetErrorResponse("用户名或者密码不正确");
            }
        }

        public KingResponse GetAdmininfoByID(int adminid)
        {
            Tb_Admin_UserInfo userinfo = Get<Tb_Admin_UserInfo>(it => it.UserID == adminid);
            if (userinfo != null)
            {
                return KingResponse.GetResponse(userinfo);
            }
            else
            {
                return KingResponse.GetErrorResponse("用户不存在");
            }
        }

        private void UpdateLastLoginDate(int userid)
        {
            string sql = "update Tb_Admin_UserInfo set LastOnlineDate=getdate() where UserID=" + userid;
            ExecuteSql(sql);
        }
    }
}
