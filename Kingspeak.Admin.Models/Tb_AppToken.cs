using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingspeak.Admin.Models
{
    public class Tb_AppToken
    {
        /// <summary>
        ///  
        /// </summary>
        public int? ID { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string AppToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public string AppDescripts { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        ///  
        /// </summary>
        public DateTime? ExpirDate { get; set; }
    }
}
