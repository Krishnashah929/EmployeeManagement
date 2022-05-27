using EM.Entity;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Login(LoginModel objloginModel)
        {
            try
            {
                //Check the user email and password
                var loggedinUser = _userService.GetByEmail(objloginModel.EmailAddress);
                return Ok(loggedinUser);
            }
            catch
            {
                return StatusCode(500);
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
        public IActionResult Register(User objUser)
        {
            try
            {
                var registerUsers = _userService.Register(objUser);
                return Ok(registerUsers);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        #endregion

        ///// <summary>
        /////  Set password method will call when user set their password from email-template
        ///// </summary>
        #region SetPassword(GET)
        [HttpGet("SetPassword/{id}")]
        public IActionResult SetPassword(int id)
        {
            try
            {
                var userDetails = _userService.GetById(id);
                return Ok(userDetails);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        #endregion

        ///// <summary>
        /////  Set password Post method will call when user enter both their password and click to set password.
        ///// </summary>
        #region SetPassword(POST)
        [HttpPost("SetPassword")]
        public IActionResult SetPassword(User user)
        {
            try
            {
                var setUserPassword = _userService.SetUserPassword(user);
                return Ok(setUserPassword);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion

        /// <summary>
        /// Reset Password get method.
        /// </summary>
        #region ForgotPassword(GET)
        [HttpGet("ForgotPassword/{id}")]
        public IActionResult ForgotPasswordAsync(int id)
        {
            try
            {
                var userDetails = _userService.GetById(id);
                return Ok(userDetails);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        #endregion

        /// <summary>
        /// Reset Password post method.
        /// </summary>
        #region ForgotPassword(POST)
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPasswordAsync(User user)
        {
            try
            {
                var setUserPassword = _userService.SetUserPassword(user);
                return Ok(setUserPassword);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        #endregion
    }
}
