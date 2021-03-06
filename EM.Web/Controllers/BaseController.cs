#region using
using EM.Common;
using EM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
#endregion

namespace EM.Web.Controllers
{
    /// <summary>
    /// Base Controller
    /// </summary>
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;
        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Method for generic api request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestType"></param>
        /// <param name="sUrl"></param>
        /// <param name="httpClient"></param>
        /// <param name="objModel"></param>
        /// <returns></returns>
        public async Task<ApiGenericModel<T>> ApiRequest<T>(RequestTypes requestType, string sUrl, HttpClient httpClient = null, object objModel = null)
        {
            HttpClient client = new HttpClient();
            {
                //Getting value from appsetting.json file
                string baseUrl = _configuration.GetValue<string>("ApiUrls:Url");

                sUrl = baseUrl + sUrl;
            }
            if (httpClient != null)
            {
                client = httpClient;
            }

            //For geeting session of jwt token
            var token = HttpContext.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            ApiGenericModel<T> objApiGenericModel = new ApiGenericModel<T>();
            try
            {
                var response = GetResponseAsync(requestType, sUrl, client, objModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    return DeserializeResponse<T>(response).Result;
                }
                else
                {
                    objApiGenericModel.StatusCode = (int)response.StatusCode;
                    objApiGenericModel.Message = await response.Content.ReadAsStringAsync();
                    response.Content?.Dispose();
                }
                return objApiGenericModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for getting response
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="sUrl"></param>
        /// <param name="client"></param>
        /// <param name="objModel"></param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> GetResponseAsync(RequestTypes requestType, string sUrl, HttpClient client, object objModel = null)
        {
            HttpResponseMessage httpResponseMessage = null;

            switch (requestType)
            {
                case RequestTypes.Get:
                    httpResponseMessage = await client.GetAsync(sUrl).ConfigureAwait(false);
                    break;
                case RequestTypes.Post:
                    httpResponseMessage = await client.PostAsJsonAsync(sUrl, objModel).ConfigureAwait(false);
                    break;
                case RequestTypes.Put:
                    httpResponseMessage = await client.PutAsJsonAsync(sUrl, objModel).ConfigureAwait(false);
                    break;
                case RequestTypes.Delete:
                    httpResponseMessage = await client.DeleteAsync(sUrl).ConfigureAwait(false);
                    break;
                default:
                    break;
            }
            if (httpResponseMessage == null)
            {

            }
            return httpResponseMessage;
        }

        /// <summary>
        /// Method for Deserialize data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpResponseMessage"></param>
        /// <returns></returns>
        private async Task<ApiGenericModel<T>> DeserializeResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            ApiResponseModel objApiResponseModel = new ApiResponseModel();
            ApiGenericModel<T> objApiGenericModel = new ApiGenericModel<T>();
            try
            {
                await httpResponseMessage.Content.ReadAsStringAsync().ContinueWith(x => objApiResponseModel = JsonConvert.DeserializeObject<ApiResponseModel>(x?.Result)).ConfigureAwait(false);

                objApiGenericModel.StatusCode = objApiResponseModel.StatusCode;
                objApiGenericModel.Message = objApiResponseModel.Message;
                objApiGenericModel.Draw = objApiResponseModel.Draw;
                objApiGenericModel.RecordsTotal = objApiResponseModel.RecordsTotal;
                objApiGenericModel.RecordsFiltered = objApiResponseModel.RecordsFiltered;
                objApiGenericModel.Token = objApiResponseModel.Token;

                //if the data is passing in object form.
                if (!string.IsNullOrEmpty(objApiResponseModel.DataObj))
                {
                    objApiGenericModel.GenericModel = JsonConvert.DeserializeObject<T>(objApiResponseModel.DataObj);
                }
                //if the data is passing in list form.
                if (!string.IsNullOrEmpty(objApiResponseModel.DataList))
                {
                    objApiGenericModel.GenericList = JsonConvert.DeserializeObject<List<T>>(objApiResponseModel.DataList);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //returning final model.
            return objApiGenericModel;
        }
    }
}