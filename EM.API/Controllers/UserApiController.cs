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
using EM.Entity;
using EM.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;

namespace EM.API.Controllers
{
    /// <summary>
    /// Api controller for User side pages 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private IUsersService _userService;
        private bool errorflag;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;

        public object HttpCacheability { get; private set; }

        [Obsolete]
        public UserApiController(IUsersService userService, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
        }

        /// <summary>
        /// Getting user list
        /// </summary>
        /// <param name="jqueryDatatableParam"></param>
        /// <returns></returns>
        #region GetUserList
        [HttpPost("GetUserList")]
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
                    //return Json(returnObj);
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
        #region AddNewUserPost
        [HttpPost("Register")]
        [Obsolete]
        public ApiResponseModel Register(User objUser)
        {
            try
            {
                if (objUser != null)
                {
                    var registerUsers = _userService.AddNewUser(objUser);
                    if (registerUsers != null)
                    {
                        //encrypt the userid for link in url.
                        var userId = EncryptionDecryption.Encrypt(objUser.UserId.ToString());

                        //link generation with userid.
                        var linkPath = "http://localhost:7399/Auth/SetPassword?link=" + userId;

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

                        SendEmail(objUser.EmailAddress, body, subject);
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, "", registerUsers);
                    }
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
        ///   Get method edit details 
        /// </summary>
        #region EditDetailsModel(GET)
        [HttpGet("EditDetailsModel/{id}")]
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
        /// <returns></returns>
        #region UpdateUserDetails
        [HttpPut("EditUser")]
        public ApiResponseModel EditUser(User user)
        {
            try
            {
                var editUser = _userService.UpdateDetails(user);
                return CommonHelper.GetResponse(HttpStatusCode.OK, "", editUser);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        ///   get delete details 
        /// </summary>
        #region DeleteDetailsModel(GET)
        [HttpGet("DeleteDetailsModel/{id}")]
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
        /// <param name="user"></param>
        /// <returns></returns>
        #region DeleteUserPost
        [HttpDelete("DeleteUser/{id}")]
        public ApiResponseModel DeleteUser(int id)
        {
            try
            {
                var deleteUser = _userService.DeleteDetails(id);
                return CommonHelper.GetResponse(HttpStatusCode.OK, "", deleteUser);
            }
             catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion

        /// <summary>
        ///  In this method we are sending main set password template to user. where they can set their new password.
        /// </summary>
        #region SendEmail
        private void SendEmail(string email, string body, string subject)
        {
            try
            {
                using (MailMessage mm = new MailMessage("krishnaa9121@gmail.com", email))
                {
                    mm.Subject = subject;
                    mm.Body = body;
                    mm.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("krishnaa9121@gmail.com", "Kri$hn@91");
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
            }
            catch (Exception)
            {
                errorflag = true;
            }
        }
        #endregion
    }
}
