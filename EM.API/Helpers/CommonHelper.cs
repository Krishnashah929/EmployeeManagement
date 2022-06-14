using EM.Models;
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
        public static  ApiResponseModel GetResponse(HttpStatusCode statusCode , string sMessage, /*string token*/ object objData = null, object objDataList = null)
        {
            return new ApiResponseModel()
            {
                StatusCode = (int)statusCode,
                Message = sMessage,
                DataObj = objData != null? JsonConvert.SerializeObject(objData) :  string.Empty,
                DataList = objDataList != null ? JsonConvert.SerializeObject(objDataList) : string.Empty,
            };
        }

        /// <summary>
        /// For getting responses for jqueryDatatable
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="recordsTotal"></param>
        /// <param name="recordsFiltered"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static ApiResponseModel GetResponseDataTable(string draw, int recordsTotal, int recordsFiltered , object Data = null)
        {
            return new ApiResponseModel()
            {
                Draw = draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                DataList = Data != null ? JsonConvert.SerializeObject(Data) : string.Empty
            };
        }


        /// <summary>
        ///  For getting jwt token 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="sMessage"></param>
        /// <param name="token"></param>
        /// <param name="objData"></param>
        /// <param name="objDataList"></param>
        /// <returns></returns>
        //public static ApiResponseModel GetResponseToken(HttpStatusCode statusCode, string sMessage, string token, object objData = null, object objDataList = null)
        //{
        //    return new ApiResponseModel()
        //    {
        //        StatusCode = (int)statusCode,
        //        Message = sMessage,
        //        DataObj = objData != null ? JsonConvert.SerializeObject(objData) : string.Empty,
        //        DataList = objDataList != null ? JsonConvert.SerializeObject(objDataList) : string.Empty,
        //        Token = token
        //    };
        //}
    }
}
