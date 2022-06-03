using EM.API.Helpers;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace EM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private IUsersService _userService;
         
        public object HttpCacheability { get; private set; }


        public UserApiController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpPost("GetUserList")]
        public ApiResponseModel GetUserList(JqueryDatatableParam jqueryDatatableParam)
        {
          
            int totalRecord = 0;
            int filterRecord = 0;
             
            var data = _userService.GetAllUser();
            if (data != null)
            {
                // get total count of records 
                totalRecord = data.Count();

                // search data when search value found
                if (!string.IsNullOrEmpty(jqueryDatatableParam.searchValue))
                {
                    data = data.Where(x =>
                      x.FirstName.ToLower().Contains(jqueryDatatableParam.searchValue.ToLower())
                      || x.Lastname.ToLower().Contains(jqueryDatatableParam.searchValue.ToLower())
                      || x.EmailAddress.ToLower().Contains(jqueryDatatableParam.searchValue.ToLower())
                    );
                }

                // get total count of records after search 
                filterRecord = data.Count();

                //sort data
                if (!string.IsNullOrEmpty(jqueryDatatableParam.sortColumn) && !string.IsNullOrEmpty(jqueryDatatableParam.sortColumnDirection))
                data = data.AsQueryable().OrderBy(jqueryDatatableParam.sortColumn + " " + jqueryDatatableParam.sortColumnDirection);

                //pagination
                var empList = data.Skip(jqueryDatatableParam.skip).Take(jqueryDatatableParam.pageSize).ToList();
                
                return CommonHelper.GetResponseDataTable(jqueryDatatableParam.draw, totalRecord, filterRecord, empList);                
                //return Json(returnObj);
            }
            //getting value from common helper.
            return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            
        }
    }
