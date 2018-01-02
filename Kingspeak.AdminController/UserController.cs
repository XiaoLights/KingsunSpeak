﻿using Kingspeak.Admin.Models;
using Kingspeak.Admin.Service;
using Kingspeak.User.Models;
using Kingspeak.User.Service;
using Kingsun.Core.Utils;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Kingspeak.AdminController
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.AppList = GetAppTokenList();
            ViewBag.AdviserList = GetAdviserList();
            return View();
        }

        public ActionResult Administrator()
        {
            return View();
        }

        public JsonResult GetUserList(int pageindex, int pagesize, string sortName, string sortOrder, string SearchKey, int? SearchType, int? Source)
        {
            PageParams<V_UserInfo> param = new PageParams<V_UserInfo>();
            UserService service = new UserService();
            param.PageSize = pagesize;
            param.PageIndex = param.GetPageIndex(pageindex, pagesize);
            param.Wheres = GetParamWheres(SearchKey, SearchType, Source);
            if (!string.IsNullOrEmpty(sortName))
            {
                param.StrOrderColumns = sortName + " " + sortOrder;
            }
            else
            {
                param.OrderColumns = it => it.CreateTime;
            }
            int totalCount = 0;
            List<V_UserInfo> list = service.GetPageList<V_UserInfo>(param, ref totalCount);
            object obj = new { total = totalCount, rows = list };
            return Json(obj);
        }


        public JsonResult GetAdminList(int pageindex, int pagesize, string sortName, string sortOrder, string SearchKey, int? SearchType)
        {
            PageParams<Tb_Admin_UserInfo> param = new PageParams<Tb_Admin_UserInfo>();
            UserService service = new UserService();
            param.PageSize = pagesize;
            param.PageIndex = param.GetPageIndex(pageindex, pagesize);
            if (SearchType.HasValue && !string.IsNullOrEmpty(SearchKey))
            {
                param.Wheres = new List<System.Linq.Expressions.Expression<Func<Tb_Admin_UserInfo, bool>>>();
                switch (SearchType)
                {
                    case 1:
                        param.Wheres.Add(it => it.UserName.Contains(SearchKey));
                        break;
                    case 2:
                        param.Wheres.Add(it => it.TrueName.Contains(SearchKey));
                        break;
                    case 3:
                        param.Wheres.Add(it => it.UserID.ToString() == SearchKey);
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
            List<Tb_Admin_UserInfo> list = service.GetPageList<Tb_Admin_UserInfo>(param, ref totalCount);
            object obj = new { total = totalCount, rows = list };
            return Json(obj);
        }

        private List<string> GetAllResource()
        {
            return null;
        }

        private List<Expression<Func<V_UserInfo, bool>>> GetParamWheres(string SearchKey, int? SearchType, int? Source)
        {
            List<Expression<Func<V_UserInfo, bool>>> where = new List<Expression<Func<V_UserInfo, bool>>>();
            if (SearchType.HasValue && !string.IsNullOrEmpty(SearchKey))
            {
                switch (SearchType)
                {
                    case 1:
                        where.Add(it => it.UserName.Contains(SearchKey));
                        break;
                    case 2:
                        where.Add(it => it.RealName.Contains(SearchKey));
                        break;
                    case 3:
                        where.Add(it => it.UserId.ToString() == SearchKey);
                        break;
                }
            }
            if (Source.HasValue && Source.Value != 0)
            {
                where.Add(it => it.ResourceID == Source);
            }
            return where;
        }

        public JsonResult SaveAdminUser(string VerifyPwd, [FromBody]Tb_Admin_UserInfo uinfo)
        {
            AdminUserService service = new AdminUserService();
            Tb_Admin_UserInfo adminuinfo = new Tb_Admin_UserInfo();
            if (uinfo.UserID.HasValue)
            {
                adminuinfo = service.Get<Tb_Admin_UserInfo>(it => it.UserID == uinfo.UserID);
                if (adminuinfo == null)
                {
                    return Json(KingResponse.GetErrorResponse("找不到要修改的用户信息"));
                }
                adminuinfo.UserName = uinfo.UserName;
                adminuinfo.TrueName = uinfo.TrueName;
                if (!string.IsNullOrEmpty(VerifyPwd))
                {
                    if (uinfo.PassWord != VerifyPwd)
                    {
                        return Json(KingResponse.GetErrorResponse("两次输入的密码不一致"));
                    }
                    adminuinfo.PassWord = StringHelper.GetMD5(uinfo.PassWord);
                }
                if (service.Update<Tb_Admin_UserInfo>(adminuinfo))
                {
                    return Json(KingResponse.GetResponse("保存成功"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("保存失败"));
                }
            }
            else
            {
                adminuinfo.UserName = uinfo.UserName;
                adminuinfo.TrueName = uinfo.TrueName;
                if (uinfo.PassWord != VerifyPwd)
                {
                    return Json(KingResponse.GetErrorResponse("两次输入的密码不一致"));
                }
                adminuinfo.PassWord = StringHelper.GetMD5(uinfo.PassWord);
                adminuinfo.UserID = StringHelper.GetUserID();
                if (service.Insert<Tb_Admin_UserInfo>(adminuinfo) > 0)
                {
                    return Json(KingResponse.GetResponse("新增成功"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("新增失败"));
                }
            }

        }

        public JsonResult SaveUser([FromBody]Tb_UserInfo uinfo)
        {
            AdminUserService service = new AdminUserService();
            Tb_UserInfo userinfo = new Tb_UserInfo();
            if (uinfo.UserId.HasValue)
            {
                userinfo = service.Get<Tb_UserInfo>(it => it.UserId == uinfo.UserId);
                if (userinfo == null)
                {
                    return Json(KingResponse.GetErrorResponse("找不到要修改的用户信息"));
                }
                var list = service.GetList<Tb_UserInfo>(it => it.UserId != uinfo.UserId && (it.UserName == uinfo.UserName || it.TelePhone == uinfo.TelePhone));
                if (list != null && list.Count > 0)
                {
                    return Json(KingResponse.GetErrorResponse("已有相同的用户名或手机号"));
                }
                userinfo.UserName = uinfo.UserName;
                userinfo.RealName = uinfo.RealName;
                userinfo.Resource = uinfo.Resource;
                userinfo.Grade = uinfo.Grade;
                userinfo.Sex = uinfo.Sex;
                userinfo.TelePhone = uinfo.TelePhone;
                if (service.Update<Tb_UserInfo>(userinfo))
                {
                    return Json(KingResponse.GetResponse("保存成功"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("保存失败"));
                }
            }
            else
            {
                var list = service.GetList<Tb_UserInfo>(it => it.UserName == uinfo.UserName || it.TelePhone == uinfo.TelePhone);
                if (list != null && list.Count > 0)
                {
                    return Json(KingResponse.GetErrorResponse("已有相同的用户名或手机号"));
                }
                userinfo.UserName = uinfo.UserName;
                userinfo.RealName = uinfo.RealName;
                userinfo.Resource = uinfo.Resource;
                userinfo.Grade = uinfo.Grade;
                userinfo.Sex = uinfo.Sex;
                userinfo.TelePhone = uinfo.TelePhone;
                userinfo.Password = StringHelper.GetMD5("123456");
                userinfo.Status = 1;
                if (service.Insert<Tb_UserInfo>(userinfo) > 0)
                {
                    return Json(KingResponse.GetResponse("新增成功"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("新增失败"));
                }
            }

        }

        public JsonResult DeleteAdminUser(string UserIDs)
        {
            AdminUserService service = new AdminUserService();
            string[] userid = UserIDs.Split(',');
            if (userid.Length > 0)
            {
                if (service.Delete<Tb_Admin_UserInfo>(userid))
                {
                    return Json(KingResponse.GetResponse("删除成功"));
                }
                else
                {
                    return Json(KingResponse.GetErrorResponse("删除失败"));
                }
            }
            return Json(KingResponse.GetErrorResponse("请传入正确的参数"));
        }

        public JsonResult SaveEditClassInfo([FromBody]Tb_UserFreeCourse ufcinfo)
        {
            AdminUserService service = new AdminUserService();
            if (!ufcinfo.ID.HasValue)
            {
                return Json(KingResponse.GetErrorResponse("请输入正确的信息"));
            }
            Tb_UserFreeCourse info = service.Get<Tb_UserFreeCourse>(ufcinfo.ID);
            if (info == null)
            {
                return Json(KingResponse.GetErrorResponse("找不到要修改的信息"));
            }
            info.ListenDate = ufcinfo.ListenDate;
            info.SignupDate = ufcinfo.SignupDate;
            info.SignupMoney = ufcinfo.SignupMoney;
            info.ClassAdviser = ufcinfo.ClassAdviser;
            if (service.Update<Tb_UserFreeCourse>(info, it => new { it.ListenDate, it.SignupDate, it.SignupMoney, it.ClassAdviser }))
            {
                return Json(KingResponse.GetResponse("修改成功"));
            }
            else
            {
                return Json(KingResponse.GetErrorResponse("修改失败"));
            }
        }

        public JsonResult GetFreeClass(int UserID)
        {
            UserService service = new UserService();
            Tb_UserInfo uinfo = service.GetList<Tb_UserInfo>(it => it.UserId == UserID).FirstOrDefault();
            if (uinfo == null)
            {
                return Json(KingResponse.GetErrorResponse("找不到用户"));
            }
            return Json(service.GetFreeClass(uinfo.TelePhone));
        }

        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        public FileResult ExportUser(string SearchKey, int? SearchType, int? Source)
        {
            UserService service = new UserService();
            HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();//创建工作簿
            string tmpTitle = "用户列表" + DateTime.Now.ToString("yyyy-MM-dd");
            List<Expression<Func<V_UserInfo, bool>>> where = GetParamWheres(SearchKey, SearchType, Source);
            List<V_UserInfo> list = service.GetList<V_UserInfo>(where);
            CreateSheet(list.OrderByDescending(x => x.AddTime).ToList(), book, tmpTitle + " ", 0, list.Count);

            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            string UserAgent = System.Web.HttpContext.Current.Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") == -1)
            {
                tmpTitle = HttpUtility.UrlEncode(tmpTitle, System.Text.Encoding.UTF8).Replace("+", "%20").Replace("%27", "'");
            }
            else
            {
                tmpTitle = "=?UTF-8?B?" + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tmpTitle)) + "?=";
            }
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/excel", tmpTitle + ".xls");
        }

        private void CreateSheet(IList<V_UserInfo> list, HSSFWorkbook book, string tmpTitle, int startIndex, int endIndex)
        {
            ISheet sheet = book.CreateSheet(tmpTitle);//创建一个名为 taskTitle 的表
            IRow headerrow = sheet.CreateRow(0);//创建一行，此行为第一行           
            ICellStyle style = book.CreateCellStyle();//创建表格样式
            style.Alignment = HorizontalAlignment.Center;//水平对齐方式
            style.VerticalAlignment = VerticalAlignment.Center;//垂直对齐方式

            //给 sheet 添加第一行的头部标题         
            headerrow.CreateCell(0).SetCellValue("用户编号");
            headerrow.CreateCell(1).SetCellValue("用户名称");
            headerrow.CreateCell(2).SetCellValue("来源");
            headerrow.CreateCell(3).SetCellValue("创建时间");
            headerrow.CreateCell(4).SetCellValue("MOD用户编号");
            headerrow.CreateCell(5).SetCellValue("用户类型");
            headerrow.CreateCell(6).SetCellValue("真实姓名");
            headerrow.CreateCell(7).SetCellValue("性别");
            headerrow.CreateCell(8).SetCellValue("添加时间");
            headerrow.CreateCell(9).SetCellValue("年级");
            headerrow.CreateCell(10).SetCellValue("状态");
            headerrow.CreateCell(11).SetCellValue("来源系统用户编号");



            for (int i = startIndex; i < endIndex; i++)
            {
                if (list[i] != null)
                {
                    V_UserInfo toinfo = list[i];
                    IRow row = sheet.CreateRow(i + 1);      //新创建一行
                    ICell cell = row.CreateCell(0);         //在新创建的一行中创建单元格
                    cell.CellStyle = style;                 //设置单元格格式
                    row.CreateCell(0).SetCellValue(toinfo.UserId.Value);
                    row.CreateCell(1).SetCellValue(toinfo.UserName);
                    row.CreateCell(2).SetCellValue(toinfo.Resource);
                    row.CreateCell(3).SetCellValue(toinfo.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    row.CreateCell(4).SetCellValue(toinfo.UserIdMod);
                    row.CreateCell(5).SetCellValue(toinfo.UserType == 1 ? "教师" : "学生");
                    row.CreateCell(6).SetCellValue(toinfo.RealName);
                    row.CreateCell(7).SetCellValue(toinfo.Sex);
                    row.CreateCell(8).SetCellValue(toinfo.AddTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                    row.CreateCell(9).SetCellValue(toinfo.Grade);
                    row.CreateCell(10).SetCellValue(toinfo.Status == 2 ? "拒绝登录" : "正常");
                    row.CreateCell(11).SetCellValue(toinfo.YUid.ToString());

                }
            }
        }

        private List<Tb_AppToken> GetAppTokenList()
        {
            UserService service = new UserService();
            return service.GetList<Tb_AppToken>(it => it.State == 1);
        }

        private List<Tb_ClassAdviser> GetAdviserList()
        {
            UserService service = new UserService();
            return service.GetList<Tb_ClassAdviser>(it => it.State == 1);
        }
    }
}
