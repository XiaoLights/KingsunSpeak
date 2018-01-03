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
            KingResponse res = service.CheckAppToken(user.Token);
            if (!res.Success)
            {
                return res;
            }
            Tb_AppToken atinfo = (Tb_AppToken)res.Data;
            if (user.UserName != user.Phone)
            {
                user.UserName = user.Phone;
            }

            uinfo.UserName = user.UserName;
            uinfo.TelePhone = user.Phone;
            uinfo.AddTime = Convert.ToDateTime(user.AddTime);
            uinfo.Grade = user.Grade;
            uinfo.Identity = user.Identity;
            uinfo.Password = user.Password;
            uinfo.RealName = user.RealName;
            uinfo.Resource = atinfo.AppName;
            uinfo.ResourceID = atinfo.ID;
            uinfo.Sex = user.Sex;
            kresponse = service.SyncUserInfo(uinfo);
            return kresponse;
        }

        /// <summary>
        /// 获取免费课程
        /// </summary>
        /// <param name="stu"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse GetFreeCourse([FromBody]StuGetFreeClass stu)
        {
            UserService service = new UserService();
            KingResponse res = service.CheckAppToken(stu.Token);
            if (!res.Success)
            {
                return res;
            }
            KingResponse kres = service.GetFreeClass(stu.StuUserName);
            return kres;
        }

        /// <summary>
        /// 获取学生列表
        /// </summary>
        /// <param name="stu"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse GetStuList([FromBody]GetStuList stu)
        {
            UserService service = new UserService();
            KingResponse res = service.CheckAppToken(stu.Token);
            if (!res.Success)
            {
                return res;
            }
            string url = "http://yzj.kingsun.cn/api/user.php?action=queryStudentListByChannelCode&token=KINGSUN_v7k6WBLPjJQfxUM6";
            var content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                    { "channelCode", stu.ChannelCode },
                    { "studentPhone", stu.StudentPhone },
                    { "startTime", stu.StartTime },
                    { "endTime", stu.EndTime },
                    { "start", stu.Start.ToString() },
                    { "limit", stu.Limit.ToString() }
                });
            YZJResponceClass result = service.GetClientResult(url, content);
            if (result.code == "200")
            {
                return KingResponse.GetResponse(result.data);
            }
            else
            {
                return KingResponse.GetErrorResponse(result.message, int.Parse(result.code));
            }

        }

        /// <summary>
        /// 获取用户登录需要的Token
        /// </summary>
        /// <param name="utoken"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse GetUserLoginToken([FromBody]GetUserToken utoken)
        {
            UserService service = new UserService();
            KingResponse res = service.CheckAppToken(utoken.Token);
            if (!res.Success)
            {
                return res;
            }
            Tb_AppToken atinfo = (Tb_AppToken)res.Data;
            YZJResponceClass result = service.GetLoginToken(utoken.UserName);
            if (result.code == "200")
            {
                return KingResponse.GetResponse(result.data);
            }
            else
            {
                return KingResponse.GetErrorResponse(result.message, int.Parse(result.code));
            }
        }

        /// <summary>
        /// 获取学生的信息
        /// </summary>
        /// <param name="stu"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse GetStuInfo([FromBody]StuGetFreeClass stu)
        {
            UserService service = new UserService();
            KingResponse res = service.CheckAppToken(stu.Token);
            if (!res.Success)
            {
                return res;
            }
            KingResponse kres = service.GetStuInfo(stu.StuUserName);
            return kres;
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        public KingResponse GetSomething(string str)
        {
            Kingsun.Core.Log4net.Log.Info("测试日志", "日志内容：" + str);
            return KingResponse.GetResponse(str);
        }
    }
}
