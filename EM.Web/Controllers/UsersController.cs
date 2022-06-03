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


/// <summary>
///Controller for all User related activites
/// </summary>
namespace EM.Web.Controllers
{
    /// <summary>
    /// Calling cache from startup.cs
    /// </summary>
    [ResponseCache(CacheProfileName = "Default0")]
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
                result = ApiRequest<User>(RequestTypes.Post, "UserApi/GetUserList", null , objJqueryDatatableParam).Result;

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

        /// <summary>
        /// UpdateUserDetails is modal for get the details of particular user.
        /// </summary>
        //#region UpdateUserDetailsGet
        //[HttpGet]
        //public IActionResult UpdateUserDetailsGet(int id)
        //{
        //    var userDetails = _schoolManagementContext.Users.Where(x => x.UserId == id).FirstOrDefault();
        //    return PartialView("_UserDetailsPartial", userDetails);
        //}
        //#endregion

        /// <summary>
        /// UpdateUserDetails is modal for updating the details of particular user.
        /// update details of users with user repository.
        /// </summary>
        #region UpdateUserDetailsPost 
        [HttpPost]
        public IActionResult UpdateUserDetailsPost(User objUpdateUser)
        {
            try
            {
                ModelState.Remove("Password");
                ModelState.Remove("RetypePassword");
                if (ModelState.IsValid)
                {
                    if (objUpdateUser != null)
                    {
                        ////Calling BaseController.
                        var result = new ApiGenericModel<User>();
                        result = ApiRequest<User>(RequestTypes.Post, "UserApi/UpdateUserDetails", null, objUpdateUser).Result;
                        if (result != null)
                        {
                            objUpdateUser = result.GenericModel;
                        }
                        if (User != null)
                        {
                            return RedirectToAction("Index", "Users");
                        }
                        else
                        {
                            
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
        /// DeleteUserDetails is modal for get the details of particular user for delete them.
        /// </summary>
        //#region DeleteUserDetailsGet
        //[HttpGet]
        //public IActionResult DeleteUserDetailsGet(int id)
        //{
        //    var deleteDetails = _schoolManagementContext.Users.Where(x => x.UserId == id).FirstOrDefault();
        //    return PartialView("_UserDeletePartial", deleteDetails);
        //}
        //#endregion

        /// <summary>
        /// DeleteUserDetailsPost is modal for deleting the particular user.
        /// Delete users with user repository.
        /// </summary>
        //#region DeleteUserDetailsPost
        //[HttpPost]
        //public IActionResult DeleteUserDetailsPost(User deleteUser)
        //{
        //    try
        //    {
        //        if (deleteUser != null)
        //        {
        //            var User = _userRepository.Delete(deleteUser);
        //            if (User != null)
        //            {
        //                return Ok(Json("true"));
        //            }
        //            else
        //            {
        //                return Ok(Json("false"));
        //            }
        //        }
        //        return NoContent();
        //    }
        //    catch (Exception)
        //    {
        //        return View("Error");
        //    }
        //}
        //#endregion
    }
}


 
/// <summary>
/// UpdateUserDetails is modal for updating the details of particular user.
/// </summary>
//#region UpdateUserDetailsPost 
//[HttpPost]
//public IActionResult UpdateUserDetailsPost(User updateUser)
//{
//Session is set into Authcontroller for userId in Set password method.
//int id = (int)HttpContext.Session.GetInt32("links");
//if (id != null)
//{
//    User updateDetails = _schoolManagementContext.Users.FirstOrDefault(x => x.UserId == objUserDetail.UserId);
//    updateDetails.FirstName = objUserDetail.FirstName;
//    updateDetails.Lastname = objUserDetail.Lastname;
//    updateDetails.EmailAddress = objUserDetail.EmailAddress;
//    updateDetails.ModifiedDate = DateTime.Now;
//    var result = _schoolManagementContext.Users.Update(updateDetails);
//    _schoolManagementContext.SaveChanges();

//    if (result != null)
//    {
//        return Ok(Json("true"));
//    }
//    return Ok(Json("false"));
//}
//return Index();

//update details of users with user repository.
//}
//#endregion

/// <summary>
/// DeleteUserDetailsPost is modal for deleting the particular user.
/// </summary>
//#region DeleteUserDetailsPost
//[HttpPost]
//public IActionResult DeleteUserDetailsPost(User deleteUser)
//{
//    //int id = (int)HttpContext.Session.GetInt32("links");
//    //if (id != null)
//    //{
//    //    User deleteDetails = _schoolManagementContext.Users.FirstOrDefault(x => x.UserId == objDeleteDetails.UserId);
//    //    deleteDetails.IsDelete = true;
//    //    deleteDetails.IsActive = false;
//    //    var result = _schoolManagementContext.Users.Update(deleteDetails);
//    //    _schoolManagementContext.SaveChanges();
//    //    if (result != null)
//    //    {
//    //        return Ok(Json("true"));
//    //    }
//    //    return Ok(Json("false"));
//    //}
//    //return Index();
//}
//#endregion
