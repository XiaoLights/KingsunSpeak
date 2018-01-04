using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingspeak.User.Models
{
    public class Tb_UserFreeCourse
    {
        public int? ID { get; set; }
        public string StuPhone { get; set; }
        public int? UserID { get; set; }
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 试听时间
        /// </summary>
        public DateTime? ListenDate { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public DateTime? SignupDate { get; set; }
        /// <summary>
        /// 报名费用
        /// </summary>
        public decimal? SignupMoney { get; set; }
        /// <summary>
        /// 课程顾问
        /// </summary>
        public string ClassAdviser { get; set; }
    }
}
