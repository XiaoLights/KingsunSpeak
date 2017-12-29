using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Kingsun.Core.Utils;
using Kingspeak.Admin.Models;

namespace Kingspeak.AdminController
{
    [MyAuthorization(IsAuth = false)]
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserInfo"] != null || Session["UserPower"] != null || Session["UserMenu"] != null)
            {
                Session.RemoveAll();
            }
            HttpCookie cookie = Request.Cookies["qmvc"];
            if (cookie != null)
            {
                ViewBag.UN = cookie["un"];
                ViewBag.PW = cookie["pw"];
                ViewBag.RM = true;
            }

            ViewBag.PageTitle = "后台管理系统";
            ViewBag.ReturnUrl = Request.QueryString["ret"];
            return View();
        }

        public JsonResult Login(string username, string password, string rememberMe)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Json(KingResponse.GetErrorResponse("请输入用户名和密码"));
            }
            if (password.StartsWith("enc_"))
            {
                password = password.Replace("enc_", "");
                password = StringHelper.Decrypt(password);
            }

            Admin.Service.LoginService loginservice = new Admin.Service.LoginService();
            KingResponse result = loginservice.AdminLogin(username, password);
            if (result.Success)
            {
                if (!string.IsNullOrEmpty(rememberMe) && rememberMe == "on")
                {
                    HttpCookie cookie = new HttpCookie("qmvc");
                    cookie["un"] = username;
                    cookie["pw"] = "enc_" + StringHelper.Encrypt(password);
                    cookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cookie);
                }
                Tb_Admin_UserInfo userinfo = (Tb_Admin_UserInfo)result.Data;
                Session["UserInfo"] = userinfo;

                HttpCookie cookielogin = new HttpCookie("lginfo");
                cookielogin["uname"] = username;
                cookielogin["uid"] = StringHelper.Encrypt(userinfo.UserID.ToString()); ;
                cookielogin.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookielogin);
            }
            return Json(result);
        }
    }

}
