using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kingspeak.Web.Models
{
    public class GetUserToken
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set;}
    }
}