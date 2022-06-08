using EM.Common;
using EM.Entity;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
///Controller for all User related activites
/// </summary>
namespace EM.Web.Controllers
{
    /// <summary>
    /// Calling cache from startup.cs
    /// </summary>
    [ResponseCache(CacheProfileName = "Default0")]
    [Authorize(Roles = "Admin")]
    public class UsersController : BaseController
    {

        /// <summary>
        /// Constructor of an object 
        /// </summary>
        public UsersController(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// After successfull login of user they will redirect on Index Page.
        /// Geeting all users with user repository.
        /// </summary>
        #region Index(GET)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Index()
        {
            //var user = _unitOfWork.UserRepository.GetAll();
            //ViewBag.users = user;
            try
            {
                if (User.Identity.IsAuthenticated == true)
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
        /// <returns></returns>
        #region GetUserList

        [HttpPost]
        public IActionResult GetUserList()
        {
            try
            {
                JqueryDatatableParam objJqueryDatatableParam = new JqueryDatatableParam();
                int totalRecord = 0;
                int filterRecord = 0;

                objJqueryDatatableParam.draw = Request.Form["draw"].FirstOrDefault();

                objJqueryDatatableParam.sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                objJqueryDatatableParam.sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                objJqueryDatatableParam.searchValue = Request.Form["search[value]"].FirstOrDefault();

                objJqueryDatatableParam.pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

                objJqueryDatatableParam.skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");


                ////Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Post, "UserApi/GetUserList", null, objJqueryDatatableParam).Result;

                var returnObj = new { draw = result.Draw, recordsTotal = result.RecordsTotal, recordsFiltered = result.RecordsFiltered, data = result.GenericList };

                if (true)
                {
                    return Json(returnObj);
                    //return RedirectToAction("Index", "Users");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Method for getting add user model
        /// </summary>
        /// <returns></returns>
        #region AddUserModelGet
        [HttpGet]
        public IActionResult AddUserModel()
        {
            try
            {
                return PartialView("_AddUserPartial");
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
        #region AddNewUserPost
        [HttpPost]
        public IActionResult Register(UpdateDetails objUpdateDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User objUser = new User();
                    objUser.FirstName = objUpdateDetails.FirstName;
                    objUser.Lastname = objUpdateDetails.Lastname;
                    objUser.EmailAddress = objUpdateDetails.EmailAddress;
                    objUser.Role = objUpdateDetails.Role;
                    //Calling BaseController.
                    var result = new ApiGenericModel<User>();
                    result = ApiRequest<User>(RequestTypes.Post, "UserApi/Register", null, objUser).Result;
                    if (result != null)
                    {
                        objUser = result.GenericModel;
                    }
                    if (true)
                    {
                        if (objUser == null)
                        {
                            TempData["Error"] = CommonValidations.RecordExistsMsg;
                            return RedirectToAction("Index", "Users");
                        }
                        TempData["Success"] = CommonValidations.NewUserRegisterd;
                        return RedirectToAction("Index", "Users");
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return RedirectToAction("Index", "Users");
        }
        #endregion

        /// <summary>
        /// Method for getting edit user model
        /// </summary>
        /// <returns></returns>
        #region EditUserModelGet
        [HttpGet]
        public IActionResult EditUserModel(int id)
        {
            try
            {
              
                //Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Get, "UserApi/EditDetailsModel/" + id).Result;

                RegisterModel User = new RegisterModel();
                User.UserId = result.GenericModel.UserId;
                User.FirstName = result.GenericModel.FirstName;
                User.Lastname = result.GenericModel.Lastname;
                User.EmailAddress = result.GenericModel.EmailAddress;
                User.Role = result.GenericModel.Role;

                if (result != null)
                {
                    return PartialView("_EditUserPartial", User);
                }
            }
             catch (Exception)
            {
                return View("Error");
            }
            return PartialView("_EditUserPartial");
        }
        #endregion

        /// <summary>
        /// UpdateUserDetails is modal for updating the details of particular user.
        /// </summary>
        #region EditUserDetailsPost 
        [HttpPost]
        public IActionResult EditUserDetailsPost(RegisterModel objRegisterModel, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (objRegisterModel != null)
                    {
                        User objUser = new User();
                        objUser.UserId = objRegisterModel.UserId;
                        objUser.FirstName = objRegisterModel.FirstName;
                        objUser.Lastname = objRegisterModel.Lastname;
                        objUser.EmailAddress = objRegisterModel.EmailAddress;
                        objUser.Role = objRegisterModel.Role;
                        ////Calling BaseController.
                        var result = new ApiGenericModel<User>();
                        result = ApiRequest<User>(RequestTypes.Put, "UserApi/EditUser", null, objRegisterModel).Result;
                        if (result != null)
                        {
                            objUser = result.GenericModel;
                        }
                        if (objUser == null)
                        {
                            TempData["Error"] = CommonValidations.RecordExistsMsg;
                            return RedirectToAction("Index", "Users");
                        }
                        else
                        {
                            TempData["Update"] = CommonValidations.UpdateUserDetails;
                            return RedirectToAction("Index", "Users");
                        }
                    }
                }
                return RedirectToAction("Index", "Users");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Method for getting delete user model
        /// </summary>
        /// <returns></returns>
        #region DeleteUserModelGet
        [HttpGet]
        public IActionResult DeleteUserModel(int id)
        {
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Get, "UserApi/DeleteDetailsModel/" + id).Result;
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
        #region DeletUserDetailsPost
        [HttpPost]
        public IActionResult DeletUserDetails(User model)
        {
            try
            {
                ////Calling BaseController.
                var result = new ApiGenericModel<User>();
                result = ApiRequest<User>(RequestTypes.Delete, "UserApi/DeleteUser/" + model.UserId).Result;
                return RedirectToAction("Index", "Users");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion
    }
}