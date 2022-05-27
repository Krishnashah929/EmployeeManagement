//using EM.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.SharePoint.Client;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace EM.Web.Controllers
//{
//    public class BaseController : Controller
//    {
//        private readonly LoginModel objloginModel;

//        public Task<ApiGenericModel<T>> ApiRequest<T>(RequestType requestType, string url, HttpClient httpClient = null, object objModel = null)

//        {
//            var sUrl = "https://localhost:44375/api/AuthApi/";

//            HttpClient client = new HttpClient();

//            var response = client.PostAsJsonAsync<LoginModel>(sUrl, objloginModel);
//            response.Wait();
//        }
//    }
//}

