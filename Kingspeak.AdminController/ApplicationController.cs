using Kingspeak.Admin.Models;
using Kingspeak.Admin.Service;
using Kingsun.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace Kingspeak.AdminController
{
    public class ApplicationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAppTokenList(int pageindex, int pagesize, string sortName, string sortOrder, string SearchKey, int? SearchType)
        {
            PageParams<Tb_AppToken> param = new PageParams<Tb_AppToken>();
            ApplicationService service = new ApplicationService();
            param.PageSize = pagesize;
            param.PageIndex = param.GetPageIndex(pageindex, pagesize);

            if (SearchType.HasValue && !string.IsNullOrEmpty(SearchKey))
            {
                param.Wheres = new List<System.Linq.Expressions.Expression<Func<Tb_AppToken, bool>>>();
                switch (SearchType)
                {
                    case 1:
                        param.Wheres.Add(it => it.ID.ToString() == SearchKey);
                        break;
                    case 2:
                        param.Wheres.Add(it => it.AppName.Contains(SearchKey));
                        break;
                    case 3:
                        param.Wheres.Add(it => it.AppToken == SearchKey);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(sortName))
            {
                param.StrOrderColumns = sortName + " " + sortOrder;
            }
            else
            {
                param.OrderColumns = it => it.CreateDate;
            }
            int totalCount = 0;
            List<Tb_AppToken> list = service.GetPageList<Tb_AppToken>(param, ref totalCount);
            object obj = new { total = totalCount, rows = list };
            return Json(obj);
        }

        public JsonResult ChangeState(int ID, int State)
        {
            ApplicationService service = new ApplicationService();
            if (service.Update<Tb_AppToken>(new Tb_AppToken { State = State, ID = ID }, it => new { it.State }))
            {
                return Json(KingResponse.GetResponse(""));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("修改失败"));
            }
        }

        public JsonResult SaveApp([FromBody]Tb_AppToken app)
        {
            if (string.IsNullOrEmpty(app.AppName) || string.IsNullOrEmpty(app.AppToken) || string.IsNullOrEmpty(app.AppDescripts))
            {
                return Json(KingResponse.GetErrorResponse("请填写完整的信息"));
            }
            ApplicationService service = new ApplicationService();
            Tb_AppToken apptoken = new Tb_AppToken();
            if (app.ID.HasValue && app.ID.Value != 0)
            {
                apptoken = service.Get<Tb_AppToken>(app.ID);
                if (apptoken == null)
                {
                    return Json(KingResponse.GetErrorResponse("App信息不存在"));
                }
                apptoken.AppName = app.AppName;
                apptoken.AppDescripts = app.AppDescripts;
                apptoken.AppToken = app.AppToken;
                apptoken.ExpirDate = app.ExpirDate;
                if (service.Update<Tb_AppToken>(apptoken))
                {
                    return Json(KingResponse.GetResponse("保存成功"));

                }
                else {
                    return Json(KingResponse.GetErrorResponse("保存失败"));
                }
            }
            else {
                apptoken.AppName = app.AppName;
                apptoken.AppDescripts = app.AppDescripts;
                apptoken.AppToken = app.AppToken;
                apptoken.ExpirDate = app.ExpirDate;
                apptoken.CreateDate = DateTime.Now;
                apptoken.State = 1;
                if (service.Insert(apptoken) > 0)
                {
                    return Json(KingResponse.GetResponse("保存成功"));

                }
                else {
                    return Json(KingResponse.GetErrorResponse("保存失败"));
                }
            }
        }
    }
}
