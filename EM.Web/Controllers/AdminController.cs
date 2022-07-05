#region using
using EM.Common;
using EM.Entity;
using EM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
#endregion

/// <summary>
///Controller for all User related activites
/// </summary>
namespace EM.Web.Controllers
{
    /// <summary>
    /// Calling cache from startup.cs
    /// </summary>
    [ResponseCache(CacheProfileName = "Default0")]
    public class AdminController : BaseController
    {
        /// <summary>
        /// Constructor of an object 
        /// </summary>
        /// <param name="configuration"></param>
        #region constructor
        public AdminController(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion

        /// <summary>
        /// After successfull login of user they will redirect on Index Page.
        /// Geeting all users with user repository.
        /// </summary>
        #region Index(GET)
        [Authorize(Roles = "Admin, Users")]
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Getting all users list from this method.
        /// </summary>
        /// <returns>all list of users</returns>
        #region GetUserList
        [HttpPost]
        [Authorize(Roles = "Admin, Users")]
        public IActionResult GetUserList()
        {
            try
            {
                if(HttpContext.Session.GetString("JWToken") == null)
                {
                    return Unauthorized();
                }
                else
                {
                    int totalRecord = 0;
                    int filterRecord = 0;
                    JqueryDatatableParam objJqueryDatatableParam = new JqueryDatatableParam();

                    objJqueryDatatableParam.draw = Request.Form["draw"].FirstOrDefault();

                    objJqueryDatatableParam.sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                    objJqueryDatatableParam.sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                    objJqueryDatatableParam.searchValue = Request.Form["search[value]"].FirstOrDefault();

                    objJqueryDatatableParam.pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

                    objJqueryDatatableParam.skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                    ////Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "AdminApi/GetUserList", null, objJqueryDatatableParam).Result;

                    var returnObj = new { draw = result.Draw, recordsTotal = result.RecordsTotal, recordsFiltered = result.RecordsFiltered, data = result.GenericList };
                    if (true)
                    {
                        return Json(returnObj);
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Method for getting user model for add and edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Add/edit new user model</returns>
        #region AddEditUserModelGet
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddEditUserModel(int? id)
        {
            RegisterModel objUser = new RegisterModel();
            try
            {
                if (id > 0)
                {
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Get, "AdminApi/EditDetailsModel/" + id).Result;
                    objUser.UserId = result.GenericModel.UserId;
                    objUser.FirstName = result.GenericModel.FirstName;
                    objUser.Lastname = result.GenericModel.Lastname;
                    objUser.EmailAddress = result.GenericModel.EmailAddress;
                    objUser.RoleId = result.GenericModel.Role;
                }
            }
            catch (Exception)
            {
                return View("Error");
            }

            return PartialView("_AddUserPartial", objUser);
        }
        #endregion

        /// <summary>
        /// For register and update user post method
        /// object of User is objUser.
        /// </summary>
        /// <param name="objRegisterModel"></param>
        /// <returns>Add new user/edit exisitng user from admin side </returns>
        #region Register/UpdateUserPost
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Register(RegisterModel objRegisterModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User objUser = new User();
                    objUser.FirstName = objRegisterModel.FirstName;
                    objUser.Lastname = objRegisterModel.Lastname;
                    objUser.EmailAddress = objRegisterModel.EmailAddress;
                    objUser.Role = objRegisterModel.RoleId;
                    objUser.UserId = objRegisterModel.UserId;
                    var result = new ApiGenericModel<User>();
                    //For add new user.
                    if (objUser.UserId == 0)
                    {
                        //Calling BaseController.
                        result = ApiRequest<User>(RequestTypes.Post, "AdminApi/Register", null, objUser).Result;
                        if (result != null)
                        {
                            objUser = result.GenericModel;
                        }
                        if (true)
                        {
                            if (objUser == null)
                            {
                                return Json(result);
                            }
                            return Json(result);
                        }
                    }
                    //For update existing user.
                    else
                    {
                        result = ApiRequest<User>(RequestTypes.Put, "AdminApi/EditUser", null, objUser).Result;
                        if (result != null)
                        {
                            objUser = result.GenericModel;
                        }
                        if (objUser == null)
                        {
                            return Json(result);
                        }
                        else
                        {
                            return Json(result);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return Content("");
        }
        #endregion

        /// <summary>
        /// Method for getting delete user model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete user model</returns>
        #region DeleteUserModelGet
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUserModel(int id)
        {
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Get, "AdminApi/DeleteDetailsModel/" + id).Result;
                if (result != null)
                {
                    return PartialView("_DeleteUserPartial", result.GenericModel);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return PartialView("_DeleteUserPartial");
        }
        #endregion

        /// <summary>
        /// Post method for delete user 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Delete user from admin side</returns>
        #region DeletUserDetailsPost
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletUserDetails(User model)
        {
            try
            {
                ////Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Delete, "AdminApi/DeleteUser/" + model.UserId).Result;
                return Json(result);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion
    }
}