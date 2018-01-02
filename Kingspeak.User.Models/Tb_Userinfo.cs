using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingspeak.User.Models
{
    public class Tb_UserInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int? UserId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public int? ResourceID { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string TelePhone { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 方直数据池编号
        /// </summary>
        public string UserIdMod { get; set; }

        /// <summary>
        /// 用户类型（1：教师 ；2：学生）
        /// </summary>
        public int? UserType { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 教师：入职日期 学生：入学时间
        /// </summary>
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 身份（当身份为教师）
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 年级（当身份为学生）
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 用户状态 1：正常 2：拒绝登录
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 第三方创建的id
        /// </summary>
        public int? YUid { get; set; }
    }
}
