using Kingspeak.Admin.Models;
using Kingsun.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kingspeak.AdminController
{
    public class MyAuthorizationAttribute : ActionFilterAttribute
    {
        private bool _IsAuth = true;
        public bool IsAuth
        {
            get { return _IsAuth; }
            set { _IsAuth = value; }
        }

        #region 事实证明Authorization并不好用
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (IsAuth)
        //    {
        //        Tb_Admin_UserInfo userinfo = (Tb_Admin_UserInfo)filterContext.HttpContext.Session["UserInfo"];
        //        if (userinfo == null)
        //        {
        //            // filterContext.Result = new RedirectResult("/Admin/Login/Login?Redirect=" + filterContext.HttpContext.Request.Url.Fragment);

        //        }
        //    }
        //    filterContext.HttpContext.Response.Write(IsAuth);
        //    //base.OnAuthorization(filterContext);
        //}
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_IsAuth)
            {
                base.OnActionExecuting(filterContext);
                if (filterContext.HttpContext.Session["UserInfo"] != null)
                {
                    filterContext.HttpContext.Session["UserInfo"] = filterContext.HttpContext.Session["UserInfo"];
                    filterContext.HttpContext.Session.Timeout = 20;
                }
                else
                {
                    HttpCookie cookieName = System.Web.HttpContext.Current.Request.Cookies.Get("lginfo");
                    if (cookieName != null)
                    {
                        //filterContext.HttpContext.Session["UserInfo"] = filterContext.HttpContext.Session["UserInfo"];
                        //filterContext.HttpContext.Session["UserPower"] = filterContext.HttpContext.Session["UserPower"];
                        string userid = cookieName["uid"];
                        userid = StringHelper.Decrypt(userid);
                        int adminid = 0;
                        if (int.TryParse(userid, out adminid))
                        {
                            Kingspeak.Admin.Service.LoginService service = new Admin.Service.LoginService();
                            Tb_Admin_UserInfo admininfo = service.Get<Tb_Admin_UserInfo>(it => it.UserID == adminid);
                            if (admininfo != null)
                            {
                                filterContext.HttpContext.Session["UserInfo"] = admininfo;
                            }
                            else
                            {
                                Redirect(filterContext);
                            }
                        }
                        else
                        {
                            Redirect(filterContext);
                        }
                    }
                    else
                    {
                        Redirect(filterContext);
                    }
                }
            }
        }

        private void Redirect(ActionExecutingContext filterContext)
        {
            //Authority 权限
            string result = string.Format("<script type='text/javascript'> window.top.location.href ='/Admin/Login/Index" + "?ret=" + filterContext.HttpContext.Request.Url.AbsolutePath + "';</script>");
            filterContext.Result = new ContentResult() { Content = result };
            return;
        }
    }
}
