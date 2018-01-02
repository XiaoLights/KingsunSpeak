using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingspeak.User.Models
{

    public class Tb_ClassAdviser
    {
        /// <summary>
        ///  
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int? ID { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string AdviserName { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string AdviserWechat { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string AdviserQrcode { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public int? State { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [SugarColumn(IsOnlyIgnoreInsert = true)]
        public DateTime? CreateDate { get; set; }

    }
}
