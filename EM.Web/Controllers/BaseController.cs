using EM.Common;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EM.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IConfiguration _configuration;
        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
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
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private static async Task<HttpResponseMessage> GetResponseAsync(RequestTypes requestType, string sUrl, HttpClient client, object objModel = null)
        {
            HttpResponseMessage httpResponseMessage = null;
            
                switch (requestType)
                {
                    case RequestTypes.Get:
                        httpResponseMessage =  await client.GetAsync(sUrl).ConfigureAwait(false);
                        break;
                    case RequestTypes.Post:
                        httpResponseMessage =  await client.PostAsJsonAsync(sUrl, objModel).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }
            if(httpResponseMessage ==  null)
            {
                 
            }
            return httpResponseMessage;
        }
       
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
             catch(Exception ex)
            {
                throw ex;
            }
            //returning final model.
            return objApiGenericModel;
        }
    }
}