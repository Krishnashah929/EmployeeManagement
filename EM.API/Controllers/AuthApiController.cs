using EM.API.Helpers;
using EM.Common;
using EM.Entity;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Mail;


namespace EM.API.Controllers
{
    /// <summary>
    /// Main all account related methods are in this AuthApi controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {

        private IUsersService _userService;

        public object HttpCacheability { get; private set; }


        public AuthApiController(IUsersService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Login for user from this post login method.
        /// loginModel is a viewmodel and used for login form.
        /// object of LoginModel is objLoginModel.
        /// </summary>
        #region Login(POST)
        [HttpPost("Login")]
        public ApiResponseModel Login(LoginModel objloginModel)
        {
            try
            {
                if (objloginModel != null)
                {
                    var loggedinUser = _userService.GetByEmail(objloginModel.EmailAddress);
                    if (loggedinUser != null)
                    {
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, "", loggedinUser);
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


        ///// <summary>
        ///// Register for user from this post Register method.
        ///// object of User is objUser.
        ///// </summary>
        #region Register(POST)
        [HttpPost("Register")]
        [Obsolete]
        public ApiResponseModel Register(User objUser)
        {
            try
            {
                if (objUser != null)
                {
                    var registerUsers = _userService.Register(objUser);
                    if (registerUsers != null)
                    {
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

        ///// <summary>
        /////  Set password method will call when user set their password from email-template
        ///// </summary>
        #region SetPassword(GET)
        [HttpGet("SetPassword/{id}")]
        public ApiResponseModel SetPassword(int id)
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

        ///// <summary>
        /////  Set password Post method will call when user enter both their password and click to set password.
        ///// </summary>
        #region SetPassword(POST)
        [HttpPost("SetPassword")]
        public ApiResponseModel SetPassword(User user)
        {
            try
            {
                if (user != null)
                {
                    var setUserPassword = _userService.SetUserPassword(user);
                    if (setUserPassword != null)
                    {
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, "", setUserPassword);
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
        /// Reset Password get method.
        /// </summary>
        #region ForgotPassword(GET)
        [HttpGet("ForgotPassword/{id}")]
        public ApiResponseModel ForgotPasswordAsync(int id)
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
        /// Reset Password post method.
        /// </summary>
        #region ForgotPassword(POST)
        [HttpPost("ForgotPassword")]
        public ApiResponseModel ForgotPasswordAsync(User user)
        {
            try
            {
                if (user != null)
                {
                    var setUserPassword = _userService.SetUserPassword(user);
                    if (setUserPassword != null)
                    {
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.OK, "", setUserPassword);
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
    }
}
