using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kingspeak.Web.Models
{
    public class Userinfo
    {
        /// <summary>
        /// 接口来源校验
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 手机号 可以是英文
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 1-教师 2-学生
        /// </summary>
        public int user_type { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string realname { get; set; }
        /// <summary>
        /// 性别 
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 教师：入职日期 学生：入学时间
        /// </summary>
        public string addtime { get; set; }
        /// <summary>
        /// 当 user_type = 1 时 (身份)
        /// </summary>
        public string identity { get; set; }
        /// <summary>
        /// 当 user_type = 2 时(年级)
        /// </summary>
        public string grade { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string channelCode { get; set; }
    }
}