using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.IO;
using System.Reflection;

namespace Kingsun.Core.Log4net
{
    public class Log
    {
        //在网站根目录下创建日志目录
        static string path;
        static string dblog = "dblog";
        static Log()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string SystemModule()
        {
            StackTrace ss = new StackTrace(true);
            //index:0为本身的方法；1为调用方法；2为其上上层，依次类推
            MethodBase mb = ss.GetFrame(2).GetMethod();
            StackFrame[] sfs = ss.GetFrames();
            string systemModule = Environment.NewLine;
            systemModule += "模块名:" + mb.Module.ToString() + ";";
            systemModule += "命名空间名:" + mb.DeclaringType.Namespace + ";";
            //仅有类名
            systemModule += "类名:" + mb.DeclaringType.Name + ";";
            systemModule += "方法名:" + mb.Name;
            return systemModule;
        }

        
        /// <summary>
        /// 向日志文件写入调试信息
        /// </summary>
        /// <param name="content">类名</param>
        /// <param name="ex">写入内容</param>
        public static void Debug(string content, Exception ex)
        {
            var methodinfo = SystemModule();
            IExtLog extLog = ExtLogManager.GetLogger(dblog);
            methodinfo += "\r\n" + content;
            extLog.Debug(methodinfo, ex);
        }

        
        /// <summary>
        /// 向日志文件写入运行时信息
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="content">写入内容</param>
        public static void Info(string className, string content)
        {
            var methodinfo = SystemModule();
            IExtLog extLog = ExtLogManager.GetLogger(dblog);
            string url = "";
            if (HttpContext.Current != null)
            {
                url = HttpContext.Current.Request.Url.ToString();
            }
            extLog.Info(GetIPAddress(), methodinfo, url, className, content);
        }

        /// <summary>
        /// 向日志文件写入出错信息
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="content">错误信息</param>
        public static void Error(string className, string content)
        {
            var methodinfo = SystemModule();
            IExtLog extLog = ExtLogManager.GetLogger(dblog);
            string url = "";
            if (HttpContext.Current != null)
            {
                url = HttpContext.Current.Request.Url.ToString();
            }
            extLog.Error(GetIPAddress(), methodinfo, url, className, content);
        }
        /// <summary>
        /// 向日志文件写入出错信息
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="ex">异常对象</param>
        public static void Error(string className, Exception ex)
        {
            var methodinfo = SystemModule();
            IExtLog extLog = ExtLogManager.GetLogger(dblog);
            extLog.Error(className, ex);
        }

        #region "获取用户IP地址"
        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            string user_IP = string.Empty;
            try
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else
                    {
                        user_IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                    }
                }
                else
                {
                    user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            catch (Exception)
            {
                return user_IP;
            }
            return user_IP;
        }
        #endregion
    }

}