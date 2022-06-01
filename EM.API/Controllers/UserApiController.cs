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

            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            var data = _userService.GetAllUser();
            if (data != null)
            {
                totalRecord = data.Count();

                // search data when search value found
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(x =>
                      x.FirstName.ToLower().Contains(searchValue.ToLower())
                      || x.Lastname.ToLower().Contains(searchValue.ToLower())
                      || x.EmailAddress.ToLower().Contains(searchValue.ToLower())
                    );
                }

                // get total count of records after search 
                filterRecord = data.Count();

                //sort data
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                data = data.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);

                //pagination
                var empList = data.Skip(skip).Take(pageSize).ToList();

                var returnObj = new { draw = draw, recordsTotal = totalRecord, recordsFiltered = filterRecord, data = empList };
                return CommonHelper.GetResponse(HttpStatusCode.OK, "", returnObj);
                //return Json(returnObj);
            }
            //getting value from common helper.
            return CommonHelper.GetResponse(HttpStatusCode.OK, "", data);
            }
            
        }
    }
