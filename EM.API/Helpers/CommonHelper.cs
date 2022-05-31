using Newtonsoft.Json;
using System.Net;
 
namespace EM.API.Helpers
{
    /// <summary>
    /// Common Helper class.
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// For getting responses 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="sMessage"></param>
        /// <param name="objData"></param>
        /// <param name="objDataList"></param>
        /// <returns></returns>
        public static  ApiResponseModel GetResponse(HttpStatusCode statusCode , string sMessage, object objData = null, object objDataList = null)
        {
            return new ApiResponseModel()
            {
                StatusCode = (int)statusCode,
                Message = sMessage,
                DataObj = objData != null? JsonConvert.SerializeObject(objData) :  string.Empty,
                DataList = objDataList != null ? JsonConvert.SerializeObject(objDataList) : string.Empty
            };
        }
    }
}
