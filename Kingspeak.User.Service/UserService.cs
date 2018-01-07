using Kingspeak.Admin.Models;
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
            //YZJResponceClass yzjresult = SyncYZJUserInfo(userinfo);
            //if (yzjresult == null)
            //{
            //    return KingResponse.GetErrorResponse("同步易助教数据失败");
            //}
            //if (yzjresult.code != "200")
            //{
            //    return KingResponse.GetErrorResponse(yzjresult.message);
            //}
            //YZJResponseUserInfo yzjuinfo = JsonHelper.DecodeJson<YZJResponseUserInfo>(yzjresult.data.ToString());
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
                _userinfo.ResourceID = userinfo.ResourceID;
                _userinfo.UserIdMod = uumsuinfo.UserID;
                _userinfo.CreateTime = DateTime.Now;
                _userinfo.AddTime = Convert.ToDateTime(userinfo.AddTime);
                _userinfo.Grade = userinfo.Grade;
                _userinfo.Identity = userinfo.Identity;
                _userinfo.Password = userinfo.Password;
                _userinfo.RealName = userinfo.RealName;
                _userinfo.Sex = userinfo.Sex;
                _userinfo.Status = 1;
                //_userinfo.Status = Convert.ToInt32(yzjuinfo.status);
                //_userinfo.YUid = Convert.ToInt32(yzjuinfo.uid);
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
                _userinfo.Status = 1;
                //_userinfo.Status = Convert.ToInt32(yzjuinfo.status);
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

        public KingResponse CheckAppToken(string appToken)
        {
            if (string.IsNullOrEmpty(appToken))
            {
                return KingResponse.GetErrorResponse("秘钥为空");
            }
            List<Tb_AppToken> atlist = GetList<Tb_AppToken>(it => it.AppToken == appToken);
            if (atlist == null || atlist.Count == 0)
            {
                return null;
            }
            if (atlist[0].State != 1)
            {
                return KingResponse.GetErrorResponse("秘钥已禁用");
            }
            if (atlist[0].ExpirDate.HasValue && atlist[0].ExpirDate < DateTime.Now)
            {
                return KingResponse.GetErrorResponse("秘钥已过期");
            }
            else
            {
                return KingResponse.GetResponse(atlist[0]);
            }
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

        public YZJResponceClass GetClientResult(string url, FormUrlEncodedContent content)
        {
            try
            {
                string result = HttpClientHelper(url, content);
                if (string.IsNullOrEmpty(result))
                {
                    return new YZJResponceClass { code = "500", message = "获取信息为空" };
                }
                Kingsun.Core.Log4net.Log.Info("获取易助教请求结果", result);
                return JsonHelper.DecodeJson<Models.YZJResponceClass>(result);
            }
            catch (Exception ex)
            {
                Kingsun.Core.Log4net.Log.Error("获取易助教请求结果错误", ex);
                return new YZJResponceClass { code = "500", message = "获取信息错误!" + ex.Message };
            }
        }

        public YZJResponceClass GetLoginToken(string UserName)
        {

            string url = "http://yzj.kingsun.cn/api/user.php?action=getToken&token=KINGSUN_v7k6WBLPjJQfxUM6";
            var content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                    { "username", UserName }
                });
            YZJResponceClass result = GetClientResult(url, content);
            object[] arr = (object[])result.data;

            if (arr.Length <= 0)
            {
                return new YZJResponceClass { code = "500", data = "", message = "获取Token失败" };
            }
            return result;
        }

        public KingResponse GetFreeClass(string stuphone)
        {
            Tb_UserInfo uinfo = GetList<Tb_UserInfo>(it => it.TelePhone == stuphone).FirstOrDefault();
            if (uinfo == null)
            {
                return KingResponse.GetErrorResponse("该用户未注册");
            }
            Tb_UserFreeCourse ufcinfo = GetList<Tb_UserFreeCourse>(it => it.UserID == uinfo.UserId).FirstOrDefault();
            if (ufcinfo != null)
            {
                return KingResponse.GetErrorResponse("该用户已领取过课程了，请勿重复操作", 001);
            }
            string url = "http://yzj.kingsun.cn/api/user.php?action=signStudentFreeClass&token=KINGSUN_v7k6WBLPjJQfxUM6";
            var content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                    { "studentPhone",stuphone }
                });
            YZJResponceClass result = GetClientResult(url, content);
            if (result.code == "200")
            {
                ufcinfo = new Tb_UserFreeCourse();
                ufcinfo.UserID = uinfo.UserId;
                ufcinfo.StuPhone = stuphone;
                ufcinfo.CreateDate = DateTime.Now;
                if (Insert<Tb_UserFreeCourse>(ufcinfo) > 0)
                {
                    return KingResponse.GetResponse("领取成功");
                }
                else
                {
                    return KingResponse.GetErrorResponse("保存领取记录失败");
                }

            }
            return KingResponse.GetErrorResponse("领取失败");
        }

        public ImportUserExcelModel InsertIntoDB(ImportUserExcelModel model)
        {
            if (model.Success.HasValue && model.Success.Value)
            {
                Tb_UserInfo uinfo = GetList<Tb_UserInfo>(it => it.UserName == model.UserName || it.TelePhone == model.TelePhone).FirstOrDefault();
                if (uinfo == null)
                {
                    uinfo = new Tb_UserInfo();
                    uinfo.UserName = model.UserName;
                    uinfo.TelePhone = model.TelePhone;
                    uinfo.Grade = model.Grade;
                    uinfo.Password = StringHelper.GetMD5("123456");
                    uinfo.RealName = model.RealName;
                    uinfo.ResourceID = model.ResourceID;
                    uinfo.Resource = model.Resource;
                    uinfo.Status = 1;
                    if (Insert<Tb_UserInfo>(uinfo) > 0)
                    {
                        uinfo = GetList<Tb_UserInfo>(it => it.UserName == model.UserName).FirstOrDefault();
                       // SyncYZJUserInfo(uinfo);
                        SyncUUMSUserInfo(uinfo);
                    }
                    else
                    {
                        model.Success = false;
                        model.ErrorMsg = "插入新用户失败";
                        return model;
                    }
                }

                Tb_UserFreeCourse cinfo = GetList<Tb_UserFreeCourse>(it => it.UserID == uinfo.UserId).FirstOrDefault();
                if (cinfo == null)
                {
                    cinfo = new Tb_UserFreeCourse();
                    cinfo.UserID = uinfo.UserId;
                    cinfo.CreateDate = DateTime.Now;
                    cinfo.StuPhone = uinfo.TelePhone;
                    cinfo.ListenDate = model.ListenDate;
                    cinfo.SignupDate = model.SignupDate;
                    cinfo.SignupMoney = model.SignupMoney;
                    cinfo.ClassAdviser = model.ClassAdviser;
                    if (Insert<Tb_UserFreeCourse>(cinfo) > 0)
                    {
                        model.Success = true;
                        return model;
                    }
                    else
                    {
                        model.Success = false;
                        model.ErrorMsg = "插入课程记录失败";
                        return model;
                    }
                }
                else
                {
                    cinfo.ListenDate = model.ListenDate;
                    cinfo.SignupDate = model.SignupDate;
                    cinfo.SignupMoney = model.SignupMoney;
                    cinfo.ClassAdviser = model.ClassAdviser;
                    if (Update<Tb_UserFreeCourse>(cinfo))
                    {
                        model.Success = true;
                        return model;
                    }
                    else
                    {
                        model.Success = false;
                        model.ErrorMsg = "更新课程记录失败";
                        return model;
                    }
                }
            }
            else
            {
                return model;
            }
        }

        public KingResponse GetStuInfo(string username)
        {
            V_UserInfo uinfo = GetList<V_UserInfo>(it => it.UserName == username && it.Status == 1 && it.UserType == 2).FirstOrDefault();
            if (uinfo == null)
            {
                uinfo = GetList<V_UserInfo>(it => it.TelePhone == username && it.Status == 1 && it.UserType == 2).FirstOrDefault();
                if (uinfo == null)
                {
                    return KingResponse.GetErrorResponse("找不到用户信息");
                }
            }
            return KingResponse.GetResponse(new
            {
                UserID = uinfo.UserId,
                UserName = uinfo.UserName,
                ApplyDate = uinfo.GetClassDate,
                ListenDate = uinfo.ListenDate,
                SignupDate = uinfo.SignupDate,
                SignupMoney = uinfo.SignupMoney,
                AdviserName = uinfo.ClassAdviser
            });
        }
    }

}
