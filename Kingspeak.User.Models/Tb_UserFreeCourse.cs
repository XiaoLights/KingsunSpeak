﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingspeak.User.Models
{
    public class Tb_UserFreeCourse
    {
        public int ID { get; set; }
        public string StuPhone { get; set; }
        public int? UserID { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}