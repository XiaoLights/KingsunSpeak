using Kingspeak.Admin.Models;
using Kingspeak.AdminController;
using Kingspeak.User.Models;
using Kingspeak.User.Service;
using Kingsun.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kingspeak.Web.Controllers
{
    [MyAuthorization(IsAuth = false)]
    public class InterfaceController : Controller
    {
        UserService service = new UserService();
        // GET: Interface
        public ActionResult Dazhi()
        {
            InitCookie();
            return View();
        }

        public ActionResult Niujin()
        {
            InitCookie();
            return View();
        }

        public ActionResult XZJTY()
        {
            ViewBag.Result = InitCookie();
            return View();
        }

        private string InitCookie()
        {
            string result = "true";
            string uname = Request.Form["username"];
            string token = Request.Form["token"];

            HttpCookie cookie = Request.Cookies.Get("client");
            if (cookie != null)
            {
                uname = cookie["uname"];
                token = cookie["token"];
                if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(token))
                {
                    return "找不到用户信息";
                }
                uname = StringHelper.Decrypt(uname, "Kingsunsoft");
                token = StringHelper.Decrypt(token, "Kingsunsoft");
                result = CheckUnaemAndToken(uname, token);
                return result;
            }
            else
            {
                result = CheckUnaemAndToken(uname, token);
                if (result != "true")
                {
                    return result;
                }
                uname = StringHelper.Encrypt(uname, "Kingsunsoft");
                token = StringHelper.Encrypt(token, "Kingsunsoft");
                cookie = new HttpCookie("client");
                cookie["uname"] = uname;
                cookie["token"] = token;
                cookie.Expires = DateTime.Now.AddMinutes(30);
                Response.Cookies.Add(cookie);
                return result;
            }

        }

        private string CheckUnaemAndToken(string uname, string token)
        {
            if (!string.IsNullOrEmpty(uname))
            {
                Tb_UserInfo uinfo = service.GetList<Tb_UserInfo>(it => it.UserName == uname).FirstOrDefault();
                if (uinfo == null)
                {
                    return "用户不存在";
                }
                if (uinfo.Status != 1)
                {
                    return "用户被禁用";
                }
                uname = StringHelper.Encrypt(uname, "Kingsunsoft");
                if (!string.IsNullOrEmpty(token))
                {
                    if (CheckToken(token))
                    {
                        return "true";
                    }
                    else
                    {
                        return "Token错误";
                    }
                }
                else
                {
                    return "找不到Token信息";
                }
            }
            else
            {
                return "找不到用户信息";
            }

        }

        private bool CheckToken(string token)
        {
            if (token == null || string.IsNullOrEmpty(token.Trim()))
            {
                return false;
            }

            KingResponse res = service.CheckAppToken(token);

            return res.Success;
        }

        public JsonResult GoToClass()
        {
            HttpCookie cookie = Request.Cookies.Get("client");
            if (cookie == null)
            {
                return Json(KingResponse.GetErrorResponse("请登录！"));
            }
            try
            {
                string username = cookie["uname"];
                //string username = "13689535120";
                username = StringHelper.Decrypt(username, "Kingsunsoft");

                var userinfo = service.GetList<Kingspeak.User.Models.Tb_UserInfo>(it => it.UserName == username).FirstOrDefault();
                if (userinfo == null)
                {
                    return Json(KingResponse.GetErrorResponse("用户不存在！"));
                }

                YZJResponceClass result = service.GetLoginToken(userinfo.UserName);
                if (result.code == "200")
                {
                    string url = System.Configuration.ConfigurationManager.AppSettings.Get("LoginToUrl");
                    string str = result.data.ToString().Substring(result.data.ToString().LastIndexOf("af_token:"));
                    str = str.Replace("af_token:", "").Replace("}", "").Replace("]", "");
                    url = url + str;
                    return Json(KingResponse.GetResponse(url));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse(result.message));
                }
            }
            catch (Exception ex)
            {
                return Json(KingResponse.GetErrorResponse("跳转失败！" + ex.Message));
            }

        }

        public JsonResult GetClass()
        {
            HttpCookie cookie = Request.Cookies.Get("client");
            if (cookie == null)
            {
                return Json(KingResponse.GetErrorResponse("请登录！"));
            }
            try
            {
                string username = cookie["uname"];
                // string username = "13689535120";
                username = StringHelper.Decrypt(username, "Kingsunsoft");

                var userinfo = service.GetList<Kingspeak.User.Models.Tb_UserInfo>(it => it.UserName == username).FirstOrDefault();
                if (userinfo == null)
                {
                    return Json(KingResponse.GetErrorResponse("用户不存在！"));
                }
                KingResponse res = service.GetFreeClass(userinfo.TelePhone);
                return Json(res);

            }
            catch (Exception ex)
            {
                return Json(KingResponse.GetErrorResponse("获取失败！" + ex.Message));
            }
        }

        public JsonResult XZJTYGoToClass()
        {
            HttpCookie cookie = Request.Cookies.Get("client");
            if (cookie == null)
            {
                return Json(KingResponse.GetErrorResponse("请登录！"));
            }
            try
            {
                string username = cookie["uname"];
                //string username = "13689535120";
                username = StringHelper.Decrypt(username, "Kingsunsoft");

                var userinfo = service.GetList<Kingspeak.User.Models.Tb_UserInfo>(it => it.UserName == username).FirstOrDefault();
                if (userinfo == null)
                {
                    return Json(KingResponse.GetErrorResponse("用户不存在！"));
                }
                Tb_UserFreeCourse classinfo = service.GetList<Tb_UserFreeCourse>(it => it.UserID == userinfo.UserId).FirstOrDefault();
                if (classinfo == null)
                {
                    return Json(KingResponse.GetErrorResponse("还未领取免费课程，请先领取免费课程再去上课", 001));
                }
                if (string.IsNullOrEmpty(classinfo.ClassAdviser))
                {
                    return Json(KingResponse.GetErrorResponse("找不到课程顾问，请扫描二维码联系课程顾问", 002));
                }

                YZJResponceClass result = service.GetLoginToken(userinfo.UserName);
                if (result.code == "200")
                {
                    string url = System.Configuration.ConfigurationManager.AppSettings.Get("LoginToUrl");
                    string str = result.data.ToString().Substring(result.data.ToString().LastIndexOf("af_token:"));
                    str = str.Replace("af_token:", "").Replace("}", "").Replace("]", "");
                    url = url + str;
                    return Json(KingResponse.GetResponse(url));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse(result.message));
                }
            }
            catch (Exception ex)
            {
                return Json(KingResponse.GetErrorResponse("跳转失败！" + ex.Message));
            }
        }
    }
}