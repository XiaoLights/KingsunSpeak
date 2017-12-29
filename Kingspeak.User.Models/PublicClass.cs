using System;
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

        public string data { get; set; }
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
}
