#region using
using EM.API.Helpers;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Linq.Dynamic.Core;
using EM.Entity;
using EM.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
#endregion

namespace EM.API.Controllers
{
    /// <summary>
    /// Api controller for admin side pages 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //For jwt token authorization
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class AdminApiController : ControllerBase
    {
        /// <summary>
        /// Generate fields for IUsersService, IHostingEnvironment, HttpCacheability,IConfiguration
        /// </summary>
        #region Fields
        private IUsersService _userService;
        private IConfiguration _configuration;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        public object HttpCacheability { get; private set; }
        #endregion

        /// <summary>
        /// Constructors for AdminApiController.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="configuration"></param>
        #region Constructors
        [Obsolete]
        public AdminApiController(IUsersService userService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            _configuration = configuration;
        }
        #endregion
        
        /// <summary>
        /// Getting user list
        /// </summary>
        /// <param name="jqueryDatatableParam"></param>
        /// <returns>all list of users</returns>
        #region GetUserList
        [HttpPost("GetUserList")]
        [Authorize(Roles = "Admin, Users")]
        public ApiResponseModel GetUserList(JqueryDatatableParam jqueryDatatableParam)
        {
            try
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
                }
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }

            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Register for user from this post Register method.
        /// object of User is objUser.
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns>Add new user form admin side</returns>
        #region AddNewUserPost
        [HttpPost("Register")]
        [Obsolete]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel Register(User objUser)
        {
            try
            {
                if (objUser != null)
                {
                    var registerUsers = _userService.AddNewUser(objUser);
                    if (registerUsers != null && objUser.UserId != 0)
                    {
                        //encrypt the userid for link in url.
                        var userId = EncryptionDecryption.Encrypt(objUser.UserId.ToString());
                        string basicUrl = _configuration.GetValue<string>("MailLinks:UrlLink");
                        //link generation with userid.
                        var linkPath = basicUrl + "SetPassword?link=" + userId;
                        //For send html page in mail
                        string webRootPath = _hostingEnvironment.WebRootPath + "/MalTemplates/SetPasswordTemplate.html";
                        StreamReader reader = new StreamReader(webRootPath);
                        string readFile = reader.ReadToEnd();
                        string myString = string.Empty;
                        myString = readFile;
                        var subject = "Set Password";
                        //when you have to replace the content of html page
                        myString = myString.Replace("@@Name@@", objUser.FirstName);
                        myString = myString.Replace("@@FullName@@", objUser.FirstName + " " + objUser.Lastname);
                        myString = myString.Replace("@@Email@@", objUser.EmailAddress);
                        myString = myString.Replace("@@Link@@", linkPath);
                        var body = myString.ToString();
                        //MailService.SendEmail(objUser.EmailAddress, body, subject);

                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.NewUserRegisterd, registerUsers);
                    }
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg, "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Get method edit details 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>open edit model model</returns>
        #region EditDetailsModel(GET)
        [HttpGet("EditDetailsModel/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel EditDetailsModel(int id)
        {
            try
            {
                var userDetails = _userService.GetById(id);
                if (userDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", userDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Post method for edit details
        /// </summary>
        /// <param name="user"></param>
        /// <returns>edit existing user details from admin side</returns>
        #region UpdateUserDetails
        [HttpPut("EditUser")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel EditUser(User user)
        {
            try
            {
                if (user != null)
                {
                    var editUser = _userService.UpdateDetails(user);
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.UpdateUserDetails, editUser);
                }
                else
                {
                    return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg);
                }
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "");
            }
        }
        #endregion

        /// <summary>
        ///   get delete details 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>open delete model</returns>
        #region DeleteDetailsModel(GET)
        [HttpGet("DeleteDetailsModel/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel DeleteDetailsModel(int id)
        {
            try
            {
                var userDetails = _userService.GetById(id);
                if (userDetails != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", userDetails);
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        /// Post method for delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>delete user details from admin side</returns>
        #region DeleteUserPost
        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel DeleteUser(int id)
        {
            try
            {
                var deleteUser = _userService.DeleteDetails(id);
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.DeleteUserDetails, deleteUser);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, CommonValidations.RecordNotExistsMsg, "");
            }
        }
        #endregion
    }
}