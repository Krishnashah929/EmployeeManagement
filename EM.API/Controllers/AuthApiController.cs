#region using
using EM.API.Helpers;
using EM.Common;
using EM.Entity;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using static EM.Common.GlobalEnum;
#endregion


namespace EM.API.Controllers
{

    /// <summary>
    /// Main all account related methods are in this AuthApi controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        /// <summary>
        /// Generate fields for IUsersService, IHostingEnvironment, IConfiguration
        /// </summary>
        #region Fields
        private IUsersService _userService;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        #endregion

        /// <summary>
        /// Constructors for AuthApiController.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="configuration"></param>
        #region Constructors
        [Obsolete]
        public AuthApiController(IUsersService userService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            _configuration = configuration;
        }

        #endregion

        /// <summary>
        /// Login for user from this post login method.
        /// loginModel is a viewmodel and used for login form.
        /// object of LoginModel is objLoginModel.
        /// </summary>
        /// <param name="objloginModel"></param>
        /// <returns>LoginModel</returns>
        #region Login(POST)
        [HttpPost("Login")]
        public ApiResponseModel Login(LoginModel objloginModel)
        {
            try
            {
                if (objloginModel != null)
                {
                    var loggedinUser = _userService.VerifyLogin(objloginModel);
                    if (loggedinUser != null)
                    {
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        int roleId = Convert.ToInt32(loggedinUser.Role);
                        string roleName = ((UserRoles)roleId).ToString();
                        //claim is used to add identity to JWT token
                        var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, loggedinUser.EmailAddress),
                        new Claim(ClaimTypes.Role, roleName),
                        new Claim("Date", DateTime.Now.ToString()),
                        };

                        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audiance"],
                        claims,    //null original value
                        expires: DateTime.Now.AddMinutes(120),

                        signingCredentials: credentials); //notBefore:

                        var GeneratedToken = new JwtSecurityTokenHandler().WriteToken(token); //return access token

                        //getting value from common helper.    
                        return CommonHelper.GetResponseToken(HttpStatusCode.OK, "", GeneratedToken, loggedinUser, "");
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
        /// Register for user from this post Register method.
        /// object of User is objUser.
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns>Register new user</returns>
        #region Register(POST)
        [HttpPost("Register")]
        [Obsolete]
        public ApiResponseModel Register(RegisterModel objRegisterModel)
        {
            try
            {
                if (objRegisterModel != null )
                {
                    var registerUsers = _userService.Register(objRegisterModel);
                    if (registerUsers != null)
                    {
                        //encrypt the userid for link in url.
                        var userId = EncryptionDecryption.Encrypt(objRegisterModel.UserId.ToString());
                        string basicUrl = _configuration.GetValue<string>("MailLinks:UrlLink");
                        //link generation with userid.
                        var linkPath = basicUrl + "SetPassword?link=" + userId;

                        string webRootPath = _hostingEnvironment.WebRootPath + "/MalTemplates/SetPasswordTemplate.html";
                        StreamReader reader = new StreamReader(webRootPath);
                        string readFile = reader.ReadToEnd();
                        string myString = string.Empty;
                        myString = readFile;
                        var subject = "Set Password";
                        //when you have to replace the content of html page
                        myString = myString.Replace("@@Name@@", objRegisterModel.FirstName);
                        myString = myString.Replace("@@FullName@@", objRegisterModel.FirstName + " " + objRegisterModel.Lastname);
                        myString = myString.Replace("@@Email@@", objRegisterModel.EmailAddress);
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
        /// Link method of sending mail for reset password. 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns>send link for forgot password</returns>
        #region Sendlink
        [HttpPost("Sendlink")]
        public ApiResponseModel Sendlink(User objUser)
        {
            try
            {
                var loggedinUser = _userService.GetByEmail(objUser.EmailAddress);
                if (loggedinUser != null)
                {
                    string basicUrl = _configuration.GetValue<string>("MailLinks:UrlLink");
                    var userId = EncryptionDecryption.Encrypt(loggedinUser.UserId.ToString());
                    //link generation with userid.
                    var linkPath = basicUrl + "ForgotPassword?link=" + userId;
                    var subject = "Password Reset Request";
                    var body = "Hi " + objUser.FirstName + ", <br/> You recently requested to reset the password for your account. " +
                               "Click the link below to reset ." + "<br/> <br/>" +
                               " <button type=' button ' class=' btn-info'> <a href='" + linkPath + "'> </a> Forgot Password </button> <br/> <br/>" +
                               "If you did not request for reset password please ignore this mail.";

                    //MailService.SendEmail(objUser.EmailAddress, body, subject);

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

        /// <summary>
        ///  Set password method will call when user set their password from email-template
        /// </summary>
        /// <param name="id"></param>
        /// <returns>open set password with particular user id</returns>
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

        /// <summary>
        ///  Set password Post method will call when user enter both their password and click to set password.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>set new password for user</returns>
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
        /// <param name="id"></param>
        /// <returns>open forgot password</returns>
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
        /// <param name="user"></param>
        /// <returns>change user's password</returns>
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
