﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingspeak.User.Models
{
    public class YZJResponceClass
    {
        public string code { get; set; }

        public string message { get; set; }

        public object data { get; set; }
    }

    public class YZJResponseUserInfo
    {
        public string uid { get; set; }
        public string RYtoken { get; set; }
        public string username { get; set; }
        public string status { get; set; }
        public string device_id { get; set; }
        public string avatars { get; set; }
        public string identity { get; set; }
        public string decription { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string utype { get; set; }
        public string utypecn { get; set; }
    }

    public class ImportUserExcelModel {
        public string UserName { get; set; }
        public DateTime? ListenDate { get; set; }
        public DateTime? SignupDate { get; set; }
        public decimal? SignupMoney { get; set; }
        public string ClassAdviser { get; set; }
        public string RealName { get; set; }
        public string TelePhone { get; set; }
        public string Grade { get; set; }
        public int? ResourceID { get; set; }
        public string Resource { get; set; }
        public bool? Success { get; set; }

        public string ErrorMsg { get; set; }
    }
}
