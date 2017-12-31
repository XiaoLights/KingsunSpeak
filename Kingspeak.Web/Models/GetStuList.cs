using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kingspeak.Web.Models
{
    public class GetStuList
    {
        /// <summary>
        /// 接口来源校验
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 请参照易助教中设置的【学生来源】
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 学生手机号码
        /// </summary>
        public string StudentPhone { get; set; }
        /// <summary>
        /// 学生注册开始日期
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 学生注册结束日期
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 开始页数
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// 获取数量
        /// </summary>
        public int Limit { get; set; }
    }
}