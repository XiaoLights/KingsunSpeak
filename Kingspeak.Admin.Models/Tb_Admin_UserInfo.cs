using System;
using System.Linq;
using System.Text;

namespace Kingspeak.Admin.Models
{
    ///<summary>
    ///用户表
    ///</summary>
    public partial class Tb_Admin_UserInfo
    {
           public Tb_Admin_UserInfo(){


           }
           /// <summary>
           /// Desc:用户编号
           /// Default:
           /// Nullable:False
           /// </summary>           
           public int UserID {get;set;}

           /// <summary>
           /// Desc:用户名称
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserName {get;set;}

           /// <summary>
           /// Desc:真实姓名
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string TrueName {get;set;}

           /// <summary>
           /// Desc:密码
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string PassWord {get;set;}

           /// <summary>
           /// Desc:头像
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string AvatarImg {get;set;}

           /// <summary>
           /// Desc:创建时间
           /// Default:DateTime.Now
           /// Nullable:True
           /// </summary>           
           public DateTime? CreateDate {get;set;}

           /// <summary>
           /// Desc:最后在线时间
           /// Default:DateTime.Now
           /// Nullable:True
           /// </summary>           
           public DateTime? LastOnlineDate {get;set;}

    }
}
