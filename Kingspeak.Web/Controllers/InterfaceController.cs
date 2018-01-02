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

        private void InitCookie()
        {
            string param = Request.Form["username"];
            string token = Request.Form["token"];

            HttpCookie cookie = Request.Cookies.Get("client");
            HttpCookie cookietoken = Request.Cookies.Get("token");
            if (string.IsNullOrEmpty(param))
            {
                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                {

                    param = StringHelper.Decrypt(cookie.Value, "Kingsunsoft");
                }
                else
                {
                    param = null;
                }
            }
            if (string.IsNullOrEmpty(token))
            {
                if (cookietoken != null && !string.IsNullOrEmpty(cookietoken.Value))
                {
                    token = StringHelper.Decrypt(cookietoken.Value, "Kingsunsoft");
                }
                else
                {
                    token = null;
                }
            }
            if (cookie == null)
            {
                cookie = new HttpCookie("client");
            }
            if (cookietoken == null)
            {
                cookietoken = new HttpCookie("token");
            }
            if (CheckToken(token))
            {
                if (string.IsNullOrEmpty(param) || string.IsNullOrEmpty(token))
                {
                    cookie.Value = "";
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                    Response.Cookies.Add(cookie);
                    cookietoken.Value = "";
                    cookietoken.Expires = DateTime.Now.AddMonths(-1);
                    Response.Cookies.Add(cookietoken);
                }
                else
                {
                    cookie.Value = StringHelper.Encrypt(param, "Kingsunsoft");
                    cookie.Expires = DateTime.Now.AddMinutes(30);
                    Response.Cookies.Add(cookie);
                    cookietoken.Value = StringHelper.Encrypt(token, "Kingsunsoft");
                    cookietoken.Expires = DateTime.Now.AddMinutes(30);
                    Response.Cookies.Add(cookietoken);
                }
            }
            else
            {
                cookie.Value = "";
                cookie.Expires = DateTime.Now.AddMonths(-1);
                Response.Cookies.Add(cookie);
                cookietoken.Value = "";
                cookietoken.Expires = DateTime.Now.AddMonths(-1);
                Response.Cookies.Add(cookietoken);
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
                string username = cookie.Value;
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
                else {
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
                string username = cookie.Value;
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
    }
}