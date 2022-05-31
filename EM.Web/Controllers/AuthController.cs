using EM.Common;
using EM.Entity;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EM.Web.Controllers
{
    /// <summary>
    /// Controller for all login and registration activites.
    /// </summary>
    public class AuthController : BaseController
    {
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private IUsersService _userService;
        private bool errorflag;
        public string baseUrl = "";

        public object HttpCacheability { get; private set; }

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="userService"></param>
        /// <param name="configuration"></param>
        [Obsolete]
        public AuthController(IHostingEnvironment hostingEnvironment, IUsersService userService, IConfiguration configuration): base(configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// This action method is for getting login modal.
        /// </summary>
        #region Login(GET)
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()

        {
            try
            {
                //if user is already logged in then they can't go back to login page
                if (User.Identity.IsAuthenticated == true)
                {
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        ///  Login method through API
        /// </summary>
        #region Login(Post)
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel objloginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userPassword = EncryptionDecryption.Encrypt(objloginModel.Password.ToString());
                    User objUser = new User();
                    objUser.EmailAddress = objloginModel.EmailAddress;
                    objUser.Password = objloginModel.Password;

                    //var url = "api/AuthApi/Login";
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "api/AuthApi/Login", null, objUser).Result;

                    if(result != null)
                    {
                        objUser = result.GenericModel;
                    }

                    if (true)
                    {                        
                        if (objUser != null)
                        {
                            var Name = objUser.FirstName + " " + objUser.Lastname;
                            if (objUser.Role == "1")
                            {
                                objUser.Role = "Admin";
                            }
                            HttpContext.Session.SetString("Userlogeddin", "true");
                            HttpContext.Session.SetString("Name", Name);

                            var userClaims = new List<Claim>()
                            {
                             new Claim("UserEmail", objloginModel.EmailAddress),
                             new Claim(ClaimTypes.Email, objloginModel.EmailAddress),
                             new Claim(ClaimTypes.Role, objUser.Role)
                            };

                            var userIdentity = new ClaimsIdentity(userClaims, "User Identity");

                            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                            HttpContext.SignInAsync(userPrincipal);

                            return RedirectToAction("Dashboard","Home");
                        }
                        else
                        {
                            TempData["Error"] = CommonValidations.RecordNotExistsMsg;
                            return View("Login");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return View();
        }
        #endregion

        /// <summary>
        ///  This action method is for getting Register modal.
        /// </summary>
        #region Register(GET)
        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                //if user is already logged in then they can't go back to register page
                if (User.Identity.IsAuthenticated == true)
                {
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Register for user from this post Register method.
        /// object of User is objUser.
        /// </summary>
        #region Register(POST)
        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Register(RegisterModel objRegisterModel)
        {
            try
            { 
                if (ModelState.IsValid)
                {
                    User objUser = new User();
                    objUser.FirstName = objRegisterModel.FirstName;
                    objUser.Lastname = objRegisterModel.Lastname;
                    objUser.EmailAddress = objRegisterModel.EmailAddress;
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "api/AuthApi/Register", null, objUser).Result;

                    if (result != null)
                    {
                        objUser = result.GenericModel;
                    }
                    if (true)
                    {
                        if (objUser != null)
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

                        }
                        else
                        {
                            TempData["Error"] = CommonValidations.RecordExistsMsg;
                            return View();
                        }
                        return RedirectToAction("Login", "Auth");
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return Ok();
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

        /// <summary>
        ///  Set password method will call when user set their password from email-template
        /// </summary>
        #region SetPassword(GET)
        [HttpGet]
        public async Task<IActionResult> SetPasswordAsync(string link , User user)
        {
            try
            {
                link = EncryptionDecryption.Decrypt(link.ToString());
                int id = Convert.ToInt32(link);
                HttpContext.Session.SetInt32("links", id);
                user.UserId = id;

                //var url = "api/AuthApi/SetPassword/" + id;
                //Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Get, "api/AuthApi/SetPassword/" + id).Result;
                if (true)
                {
                    return View(result.GenericModel);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        ///  Set password Post method will call when user enter both their password and click to set password.
        /// </summary>
        #region SetPassword(POST)
        [HttpPost]
        public async Task<IActionResult> SetPasswordAsync(SetPassword objSetPassword)
        {
            try
            {
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    User user = new User();
                    user.Password = objSetPassword.Password;
                    user.RetypePassword = objSetPassword.RetypePassword;

                    int id = (int)HttpContext.Session.GetInt32("links");
                    user.UserId = id;
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "api/AuthApi/SetPassword", null, user).Result;

                    if (result != null)
                    {
                        user = result.GenericModel;
                    }
                    if (true)
                    {
                        if (user != null)
                        {
                            return RedirectToAction("Login", "Auth");
                        }
                        else
                        {
                            message = CommonValidations.RecordExistsMsg;
                            return Content(message);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return View();
        }
        #endregion

        /// <summary>
        /// Open forgot password model for sendlink on entered e=mail
        /// </summary>
        #region GetForgotPasswordModel
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPasswordModel()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Link method of sending mail for reset password. 
        /// </summary>
        #region Sendlink
        [HttpPost]
        public IActionResult Sendlink(User objUser)
        {
            var loggedinUser = _userService.GetByEmail(objUser.EmailAddress);

            var userId = EncryptionDecryption.Encrypt(loggedinUser.UserId.ToString());
            //link generation with userid.
            var linkPath = "http://localhost:7399/Auth/ForgotPassword?link=" + userId;

            if (loggedinUser != null)
            {
                var subject = "Password Reset Request";
                var body = "Hi " + objUser.FirstName + ", <br/> You recently requested to reset the password for your account. " +
                           "Click the link below to reset ." + "<br/> <br/><a href='" + linkPath + "'>" + linkPath + "</a> <br/> <br/>" +
                           "If you did not request for reset password please ignore this mail.";
                SendEmail(objUser.EmailAddress, body, subject);

                TempData["linkSendMsg"] = CommonValidations.LinkSendMsg;
                return RedirectToAction("ForgotPasswordModel", "Auth");
            }
            else
            {
                TempData["WrongMailMsg"] = CommonValidations.WrongMailMsg;
                return RedirectToAction("ForgotPasswordModel", "Auth");
            }
        }
        #endregion

        /// <summary>
        /// Reset Password get method.
        /// </summary>
        #region ForgotPassword(GET)
        [HttpGet]
        public async Task<IActionResult> ForgotPasswordAsync(User user, string link)
        {
            try
            {
                link = EncryptionDecryption.Decrypt(link.ToString());
                int id = Convert.ToInt32(link);
                HttpContext.Session.SetInt32("links", id);
                user.UserId = id;
                //Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Get, "api/AuthApi/ForgotPassword/" + id).Result;
                if (true)
                {
                    return View(result.GenericModel);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Reset Password post method.
        /// </summary>
        #region ForgotPassword(POST)
        [HttpPost]
        public async Task<ActionResult> ForgotPasswordAsync(User user)
        {
            try
            {
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    int id = (int)HttpContext.Session.GetInt32("links");
                    user.UserId = id;
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "api/AuthApi/SetPassword", null, user).Result;

                    if (result != null)
                    {
                        user = result.GenericModel;
                    }
                    if (true)
                    {
                        if (user != null)
                        {
                            TempData["successMsg"] = CommonValidations.PasswordUpdateMsg;
                        }
                        else
                        {
                            TempData["failureMsg"] = CommonValidations.PasswordNotUpdateMsg;
                        }
                    }
                }
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Access denined method.
        /// if user is unauthenticate then will reirect to this method.
        /// </summary>
        #region Accessdenined
        public ActionResult Error()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// When user logout from their account.
        /// Set session "Userlogeddin" as false when user logged out from their session.
        /// After logged out session will be clear and user will be redirect to Login.
        /// </summary>
        #region LogOut
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                HttpContext.Session.SetString("Userlogeddin", "false");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Auth");
            }
            catch
            {
                return View("Error");
            }
        }
        #endregion
    }
}
