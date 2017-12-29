using Kingspeak.Admin.Models;
using Kingspeak.User.Models;
using Kingspeak.User.Service;
using Kingspeak.Web.Models;
using Kingsun.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;


namespace Kingspeak.Web.Controllers
{
    public class UserInterfaceController : ApiController
    {

        /// <summary>
        /// 同步用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse SyncUserInfo([FromBody]Userinfo user)
        {
            UserService service = new UserService();
            KingResponse kresponse = new KingResponse();
            Tb_UserInfo uinfo = new Kingspeak.User.Models.Tb_UserInfo();
            Tb_AppToken atinfo = service.CheckAppToken(user.token);
            if (atinfo == null)
            {
                return KingResponse.GetErrorResponse("Token秘钥错误");
            }
            if (user.username != user.phone)
            {
                user.username = user.phone;
            }

            uinfo.UserName = user.username;
            uinfo.TelePhone = user.phone;
            uinfo.AddTime = Convert.ToDateTime(user.addtime);
            uinfo.Grade = user.grade;
            uinfo.Identity = user.identity;
            uinfo.Password = user.password;
            uinfo.RealName = user.realname;
            uinfo.Resource = atinfo.AppName;
            uinfo.ResourceID = atinfo.ID;
            uinfo.Sex = user.sex;
            kresponse = service.SyncUserInfo(uinfo);
            return kresponse;
        }
    }
}
