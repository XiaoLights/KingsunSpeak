using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.Core.Utils
{
    public class KingResponse
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 操作结果数据
        /// </summary>
        public object Data { get; set; }


        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }


        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get;
            set;
        }


        /// <summary>
        /// 按错误数据创建输出对象
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static KingResponse GetErrorResponse(string errorMsg, int errorCode = 0)
        {
            KingResponse response = new KingResponse();
            response.Success = false;
            response.ErrorCode = errorCode;
            response.ErrorMsg = errorMsg;
            response.Data = null;
            return response;
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static KingResponse GetResponse(object data)
        {
            KingResponse res = new KingResponse();
            res.Success = true;
            res.Data = data;
            return res;
        }
    }
}
