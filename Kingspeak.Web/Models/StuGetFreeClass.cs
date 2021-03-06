﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kingspeak.Web.Models
{
    public class StuGetFreeClass
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 学生用户名或者手机号
        /// </summary>
        public string StuUserName { get; set; }
    }
}