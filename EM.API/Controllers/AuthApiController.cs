using EM.API.Helpers;
using EM.Common;
using EM.Entity;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        public object HttpCacheability { get; private set; }

        [Obsolete]
        public AuthApiController(IUsersService userService, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
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
                    //var loggedinUser = _userService.GetByEmail(objloginModel.EmailAddress);
                    User objUser = new User();
                    objUser.EmailAddress = objloginModel.EmailAddress;
                    objUser.Password = objloginModel.Password;

                    var loggedinUser = _userService.VerifyLogin(objUser);
                    if (loggedinUser != null)
                    {
                        if(loggedinUser.Role == "1")
                        {
                            loggedinUser.Role = "Admin";
                        }
                        else if(loggedinUser.Role == "2")
                        {
                            loggedinUser.Role = "User";
                        }
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
                    if (registerUsers != null && objUser.UserId != 0)
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

                        //SendEmail(objUser.EmailAddress, body, subject);
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
                        NetworkCredential NetworkCred = new NetworkCredential("krishnaa9121@gmail.com", "Kri$hn@@91");
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Link method of sending mail for reset password. 
        /// </summary>
        #region Sendlink
        [HttpPost("Sendlink")]
        public ApiResponseModel Sendlink(User objUser)
        {
            try
            {
                var loggedinUser = _userService.GetByEmail(objUser.EmailAddress);
                if (loggedinUser != null)
                {
                    var userId = EncryptionDecryption.Encrypt(loggedinUser.UserId.ToString());
                    //link generation with userid.
                    var linkPath = "http://localhost:7399/Auth/ForgotPassword?link=" + userId;
                    var subject = "Password Reset Request";
                    var body = "Hi " + objUser.FirstName + ", <br/> You recently requested to reset the password for your account. " +
                               "Click the link below to reset ." + "<br/> <br/><a href='" + linkPath + "'>" + linkPath + "</a> <br/> <br/>" +
                               "If you did not request for reset password please ignore this mail.";
                    SendEmail(objUser.EmailAddress, body, subject);

                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", loggedinUser);
                }
                else
                {
                    return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "", "");
                }
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
