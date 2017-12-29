﻿using Kingspeak.Admin.Models;
using Kingspeak.User.Models;
using Kingsun.Core.Log4net;
using Kingsun.Core.Utils;
using Kingsun.Framework.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kingspeak.User.Service
{
    public class UserService : SqlSugarManager
    {
        public KingResponse SyncUserInfo(Models.Tb_UserInfo userinfo)
        {
            YZJResponceClass yzjresult = SyncYZJUserInfo(userinfo);
            if (yzjresult == null)
            {
                return KingResponse.GetErrorResponse("同步易助教数据失败");
            }
            if (yzjresult.code != "200")
            {
                return KingResponse.GetErrorResponse(yzjresult.message);
            }
            YZJResponseUserInfo yzjuinfo = JsonHelper.DecodeJson<YZJResponseUserInfo>(yzjresult.data);
            UUMSUserService.User uumsuinfo = SyncUUMSUserInfo(userinfo);
            if (uumsuinfo == null)
            {
                return KingResponse.GetErrorResponse("同步UUMS数据失败");
            }

            Tb_UserInfo _userinfo = GetList<Tb_UserInfo>(it => it.UserName == userinfo.UserName).FirstOrDefault();
            if (_userinfo == null)
            {
                _userinfo = new Tb_UserInfo();
                _userinfo.UserName = userinfo.UserName;
                _userinfo.Resource = userinfo.Resource;
                _userinfo.Resource = userinfo.Resource;
                _userinfo.ResourceID = userinfo.ResourceID;
                _userinfo.UserIdMod = uumsuinfo.UserID;
                _userinfo.CreateTime = DateTime.Now;
                _userinfo.AddTime = Convert.ToDateTime(userinfo.AddTime);
                _userinfo.Grade = userinfo.Grade;
                _userinfo.Identity = userinfo.Identity;
                _userinfo.Password = userinfo.Password;
                _userinfo.RealName = userinfo.RealName;
                _userinfo.Sex = userinfo.Sex;
                _userinfo.Status = Convert.ToInt32(yzjuinfo.status);
                _userinfo.YUid = Convert.ToInt32(yzjuinfo.uid);
                _userinfo.UserType = userinfo.UserType;
                if (Insert<Tb_UserInfo>(_userinfo) > 0)
                {
                    return KingResponse.GetResponse("");
                }
                else
                {
                    return KingResponse.GetErrorResponse("插入数据库失败");
                }
            }
            else
            {
                _userinfo.Grade = userinfo.Grade;
                _userinfo.Password = userinfo.Password;
                _userinfo.RealName = userinfo.RealName;
                _userinfo.Sex = userinfo.Sex;
                _userinfo.UserIdMod = uumsuinfo.UserID;
                _userinfo.Status = Convert.ToInt32(yzjuinfo.status);
                _userinfo.UserType = userinfo.UserType;
                if (Update<Tb_UserInfo>(_userinfo))
                {
                    return KingResponse.GetResponse("");
                }
                else
                {
                    return KingResponse.GetErrorResponse("更新数据库失败");
                }
            }




        }

        public Tb_AppToken CheckAppToken(string appToken)
        {
            if (string.IsNullOrEmpty(appToken))
            {
                return null;
            }
            List<Tb_AppToken> atlist = GetList<Tb_AppToken>(it => it.AppToken == appToken);
            if (atlist == null)
            {
                return null;
            }
            return atlist[0];
        }

        public YZJResponceClass SyncYZJUserInfo(Tb_UserInfo userinfo)
        {
            //发送请求到易助教
            string url = "http://yzj.kingsun.cn/api/user.php?action=signup&token=KINGSUN_v7k6WBLPjJQfxUM6";
            var content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                    { "username", userinfo.TelePhone },
                    { "phone", userinfo.TelePhone },
                    { "user_type", userinfo.Identity.ToString() },
                    { "password", userinfo.Password },
                    { "realname", userinfo.RealName },
                    { "sex", userinfo.Sex },
                    { "addtime", userinfo.AddTime.ToString() },
                    { "identity", userinfo.Identity },
                    { "grade", userinfo.Grade },
                    { "channelCode",  "" }
                });
            string result = HttpClientHelper(url, content);
            if (string.IsNullOrEmpty(result))
            {
                return new YZJResponceClass { code = "500", message = "新增数据失败" };
            }
            else
            {
                YZJResponceClass yzjresult = JsonHelper.DecodeJson<Models.YZJResponceClass>(result);
                if (yzjresult.code != "201")
                {
                    if (!(yzjresult.code == "422" && yzjresult.message.Contains("username已存在")))
                    {
                        return yzjresult;
                    }
                    else
                    {
                        url = "http://yzj.kingsun.cn/api/user.php?action=updateInfo&token=KINGSUN_v7k6WBLPjJQfxUM6";
                        content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                               { "username", userinfo.TelePhone },
                                { "phone", userinfo.TelePhone },
                                { "user_type", userinfo.Identity.ToString() },
                                { "password", userinfo.Password },
                                { "realname", userinfo.RealName },
                                { "sex", userinfo.Sex },
                                { "addtime", userinfo.AddTime.ToString() },
                                { "identity", userinfo.Identity },
                                { "grade", userinfo.Grade },
                                { "channelCode",  "" }
                            });
                        result = HttpClientHelper(url, content);
                        if (string.IsNullOrEmpty(result))
                        {
                            return new YZJResponceClass { code = "500", message = "更新数据失败" };
                        }
                        else
                        {
                            yzjresult = JsonHelper.DecodeJson<Models.YZJResponceClass>(result);
                            if (yzjresult.code != "200")
                            {
                                return yzjresult;
                            }
                        }
                    }
                }
                return yzjresult;
            }


        }

        public UUMSUserService.User SyncUUMSUserInfo(Tb_UserInfo userinfo)
        {
            UUMSUserService.FZUUMS_UserService service = new UUMSUserService.FZUUMS_UserService();
            UUMSUserService.User uinfo = service.GetUserInfoByName("KS0205", userinfo.UserName);
            if (uinfo == null)
            {
                uinfo = service.GetUserInfoByTelephone("KS0205", userinfo.UserName);
                if (uinfo == null)
                {
                    uinfo = service.InserUserSingle("KS0205", new UUMSUserService.User
                    {
                        UserName = userinfo.UserName,
                        PassWord = userinfo.Password,
                        Telephone = userinfo.TelePhone,
                        UserType = userinfo.UserType == 1 ? 12 : 26,
                        RegDate = DateTime.Now
                    });
                }
            }
            if (uinfo != null)
            {
                UUMSUserService.User uinfo2 = new UUMSUserService.User();
                uinfo2.UserID = uinfo.UserID;
                uinfo2.Telephone = userinfo.TelePhone;
                uinfo2.TrueName = userinfo.TelePhone;
                uinfo2.PassWord = userinfo.Password;
                UUMSUserService.ReturnInfo rinfo = service.UpdateUserInfo2("KS0205", uinfo2);
                if (!rinfo.Success)
                {
                    Log.Info("同步用户到UUMS失败", rinfo.ErrorMsg);
                }
            }
            return uinfo;

        }

        private string HttpClientHelper(string url, FormUrlEncodedContent content)
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };
            using (var httpclient = new HttpClient(handler))
            {
                httpclient.BaseAddress = new Uri(url);

                var task = httpclient.PostAsync(url, content);
                if (task.Result.IsSuccessStatusCode)
                {
                    var rep = task.Result;//在这里会等待task返回。
                    var task2 = rep.Content.ReadAsStringAsync();//读取响应内容
                    return task2.Result;
                }
            }
            return "";
        }
    }
}