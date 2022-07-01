#region using
using EM.Common;
using EM.Entity;
using EM.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using static EM.Common.GlobalEnum;
#endregion

namespace EM.Web.Controllers
{
    /// <summary>
    /// Controller for all login and registration activites.
    /// </summary>
    public class AuthController : BaseController
    {
        /// <summary>
        /// Generate fields for IConfiguration, IToastNotification
        /// </summary>
        #region Fields
        public string baseUrl = string.Empty;
        private readonly IToastNotification _toastNotification;
        public object HttpCacheability { get; private set; }
        #endregion

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="toastNotification"></param>
        #region Constructor
        public AuthController(IConfiguration configuration, IToastNotification toastNotification) : base(configuration)
        {
            _toastNotification = toastNotification;
        }
        #endregion

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
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Admin");
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
        ///  Login method through API
        /// </summary>
        /// <param name="objloginModel"></param>
        /// <returns>Login model </returns>
        #region Login(Post)
        [HttpPost]
        public IActionResult Login(LoginModel objloginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "AuthApi/Login", null, objloginModel).Result;

                    if (result.StatusCode != 400)
                    {
                        var objUser = result.GenericModel;
                        //Set session of jwt token
                        HttpContext.Session.SetString("JWToken", result.Token);

                        if (objUser != null)
                        {
                            var Name = objUser.FirstName + " " + objUser.Lastname;
                            HttpContext.Session.SetString("Name", Name);
                            int roleId = Convert.ToInt32(objUser.Role);
                            string roleName = ((UserRoles)roleId).ToString();
                            var userClaims = new List<Claim>()
                            {
                             new Claim("UserEmail", objloginModel.EmailAddress),
                             new Claim(ClaimTypes.Email, objloginModel.EmailAddress),
                             new Claim(ClaimTypes.Role, roleName)
                            };

                            var userIdentity = new ClaimsIdentity(userClaims, "User Identity");
                            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                            HttpContext.SignInAsync(userPrincipal);
                            if (roleName == "Admin" && objUser.IsDelete == false)
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else if (roleName == "Users" && objUser.IsDelete == false)
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                            else if (roleName == "Receptionist" && objUser.IsDelete == false)
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                        }
                        else
                        {
                            _toastNotification.AddErrorToastMessage(CommonValidations.InvalidUserMsg);
                            return View("Login");
                        }
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage(CommonValidations.InvalidUserMsg);
                        return View("Login");
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
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Admin");
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
        /// <param name="objRegisterModel"></param>
        /// <returns>Register new user</returns>
        #region Register(POST)
        [HttpPost]
        public IActionResult Register(RegisterModel objRegisterModel)
        {
            try
            {
                this.ModelState.Remove("RoleId");
                if (ModelState.IsValid)
                { 
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "AuthApi/Register", null, objRegisterModel).Result;
                    if (result != null)
                    {
                        var objUser = result.GenericModel;
                    }
                    if (true)
                    {
                        if (result.GenericModel == null)
                        {
                            _toastNotification.AddErrorToastMessage(CommonValidations.RecordExistsMsg);
                            return View();
                        }
                        _toastNotification.AddSuccessToastMessage(CommonValidations.NewUserRegisterd);
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
        ///  Set password method will call when user set their password from email-template
        /// </summary>
        /// <param name="link"></param>
        /// <param name="user"></param>
        /// <returns>open set password for user</returns>
        #region SetPassword(GET)
        [HttpGet]
        public IActionResult SetPassword(string link, User user)
        {
            try
            {
                link = EncryptionDecryption.Decrypt(link.ToString());
                int id = Convert.ToInt32(link);
                HttpContext.Session.SetInt32("links", id);
                user.UserId = id;
                //Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Get, "AuthApi/SetPassword/" + id).Result;
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
        /// <param name="objSetPassword"></param>
        /// <returns>Set password for user</returns>
        #region SetPassword(POST)
        [HttpPost]
        public IActionResult SetPassword(User user)
        {
            try
            {
                ModelState.Remove("FirstName");
                ModelState.Remove("Lastname");
                ModelState.Remove("EmailAddress");
                if (ModelState.IsValid)
                {
                    
                    int id = (int)HttpContext.Session.GetInt32("links");
                    user.UserId = id;
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "AuthApi/SetPassword", null, user).Result;

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
                            _toastNotification.AddErrorToastMessage(CommonValidations.RecordExistsMsg);
                            return View();
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
        /// <param name="objUser"></param>
        /// <returns>send link for forgot password</returns>
        #region Sendlink
        [HttpPost]
        public IActionResult Sendlink(User objUser)
        {

            //Calling BaseController.
            var result = new ApiGenericModel<User>();
            result = ApiRequest<User>(RequestTypes.Post, "AuthApi/Sendlink", null, objUser).Result;
            if (result != null)
            {
                objUser = result.GenericModel;
            }
            if (true)
            {
                if (objUser != null)
                {
                    _toastNotification.AddSuccessToastMessage(CommonValidations.LinkSendMsg);
                    return RedirectToAction("ForgotPasswordModel", "Auth");
                }
            }
            _toastNotification.AddErrorToastMessage(CommonValidations.InvalidUserMsg);
            return RedirectToAction("ForgotPasswordModel", "Auth");
        }
        #endregion

        /// <summary>
        /// Reset Password get method.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="link"></param>
        /// <returns>open forgot password</returns>
        #region ForgotPassword(GET)
        [HttpGet]
        public IActionResult ForgotPassword(User user, string link)
        {
            try
            {
                link = EncryptionDecryption.Decrypt(link.ToString());
                int id = Convert.ToInt32(link);
                HttpContext.Session.SetInt32("links", id);
                user.UserId = id;
                //Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Get, "AuthApi/ForgotPassword/" + id).Result;
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
        /// <param name="user"></param>
        /// <returns>change user's password</returns>
        #region ForgotPassword(POST)
        [HttpPost]
        public ActionResult ForgotPassword(User user)
        {
            try
            {
                ModelState.Remove("FirstName");
                ModelState.Remove("Lastname");
                ModelState.Remove("EmailAddress");
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    int id = (int)HttpContext.Session.GetInt32("links");
                    user.UserId = id;
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "AuthApi/SetPassword", null, user).Result;
                    if (result != null)
                    {
                        user = result.GenericModel;
                    }
                    if (true)
                    {
                        if (user != null)
                        {
                            _toastNotification.AddErrorToastMessage(CommonValidations.PasswordUpdateMsg);
                            return View();
                        }
                        else
                        {
                            _toastNotification.AddErrorToastMessage(CommonValidations.PasswordNotUpdateMsg);
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