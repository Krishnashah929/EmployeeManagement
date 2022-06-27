#region Using
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

namespace EM.Web.Controllers
{
    public class DoctorController : BaseController
    {
        /// <summary>
        /// Constructor of an object 
        /// </summary>
        /// <param name="configuration"></param>
        #region constructor
        public DoctorController(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion

        /// <summary>
        /// Get full calender from this method.
        /// </summary>
        /// <returns>full calender</returns>
        #region TimeSheets
        [HttpGet]
        public IActionResult TimeSheets()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// Get index of doctor's page
        /// </summary>
        /// <returns>MAin doctor's index page</returns>
        #region GetDoctors
        [HttpGet]
        public IActionResult GetDoctors()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// Getting all doctors list from this method.
        /// </summary>
        /// <returns>all list of users</returns>
        #region GetDoctorsList
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDoctorsList()
        {
            try
            {
                if (HttpContext.Session.GetString("JWToken") == null)
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
                    var result = new ApiGenericModel<Doctor>();
                    result = ApiRequest<Doctor>(RequestTypes.Post, "DoctorApi/GetDoctorList", null, objJqueryDatatableParam).Result;

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
        /// Get Doctor 
        /// </summary>
        /// <returns>List of countries</returns>
        #region GetDoctorForAppointment
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetDoctor()
        {
            //Calling BaseController.
            var result = new ApiGenericModel<Doctor>();
            result = ApiRequest<Doctor>(RequestTypes.Get, "DoctorApi/GetDoctor").Result;
            return new JsonResult(result.GenericList);
        }
        #endregion

        /// <summary>
        /// Get Patient
        /// </summary>
        /// <returns>List of Patient</returns>
        #region GetPatient
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetPatient()
        {
            //Calling BaseController.
            var result = new ApiGenericModel<User>();
            result = ApiRequest<User>(RequestTypes.Get, "DoctorApi/GetPatient").Result;
            return new JsonResult(result.GenericList);
        }
        #endregion

        /// <summary>
        /// Method for edit doctor details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Edit Doctor model</returns>
        #region EditDoctorsModel
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditDoctorsModel(int? id, int userId)
        {
            Doctor objDoctor = new Doctor();
            try
            {
                if (id > 0 && userId != null)
                {
                    //Calling BaseController.
                    var result = new ApiGenericModel<Doctor>();
                    result = ApiRequest<Doctor>(RequestTypes.Get, "DoctorApi/EditDoctorModel/" + id).Result;
                    objDoctor.UserId = result.GenericModel.UserId;
                    objDoctor.DoctorId = result.GenericModel.DoctorId;
                    objDoctor.FirstName = result.GenericModel.FirstName;
                    objDoctor.Lastname = result.GenericModel.Lastname;
                    objDoctor.EmailAddress = result.GenericModel.EmailAddress;
                    objDoctor.PhoneNumber = result.GenericModel.PhoneNumber;
                    objDoctor.Pincode = result.GenericModel.Pincode;
                    objDoctor.Address = result.GenericModel.Address;
                    objDoctor.CityID = result.GenericModel.CityID;
                    objDoctor.StateID = result.GenericModel.StateID;
                    objDoctor.CountryID = result.GenericModel.CountryID;
                    objDoctor.Color = result.GenericModel.Color;
                    objDoctor.SpecialityId = result.GenericModel.SpecialityId;
                }
            }
            catch (Exception)
            {
                return View("Error");
            }

            return PartialView("_EditDoctorsPartial", objDoctor);
        }
        #endregion

        /// <summary>
        /// For register and update doctor post method
        /// object of doctor is objDoctor.
        /// </summary>
        /// <param name="objDoctor"></param>
        /// <returns>Add new edit exisitng doctor from admin side </returns>
        #region EditDoctor
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditDoctor(Doctor objDoctor)
        {
            try
            {
                var result = new ApiGenericModel<Doctor>();

                result = ApiRequest<Doctor>(RequestTypes.Put, "DoctorApi/EditDoctor", null, objDoctor).Result;
                if (result != null)
                {
                    objDoctor = result.GenericModel;
                }
                if (objDoctor == null)
                {
                    return Json(result);
                }
                else
                {
                    return Json(result);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Get Country 
        /// </summary>
        /// <returns>List of countries</returns>
        #region GetCountry
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetCountry()
        {
            //Calling BaseController.
            var result = new ApiGenericModel<Country>();
            result = ApiRequest<Country>(RequestTypes.Get, "DoctorApi/GetCountry").Result;
            return new JsonResult(result.GenericList);
        }
        #endregion

        /// <summary>
        /// Get GetState 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of states</returns>
        #region GetState
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetState(int id)
        {
            //Calling BaseController.
            var result = new ApiGenericModel<State>();
            result = ApiRequest<State>(RequestTypes.Get, "DoctorApi/GetState/" + id).Result;
            return new JsonResult(result.GenericList);
        }
        #endregion

        /// <summary>
        /// Get GetCity 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of cities</returns>
        #region GetCity
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetCity(int id)
        {
            //Calling BaseController.
            var result = new ApiGenericModel<City>();
            result = ApiRequest<City>(RequestTypes.Get, "DoctorApi/GetCity/" +id).Result;
            return new JsonResult(result.GenericList);
        }
        #endregion

        /// <summary>
        ///  Post Appontment .
        /// </summary>
        /// <param name="objAppointment"></param>
        /// <returns>Add Appointment for user. </returns>
        #region PostAppointment
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult PostAppointment(Appointment objAppointment)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = new ApiGenericModel<Appointment>();
                    //For add appointment.

                    result = ApiRequest<Appointment>(RequestTypes.Put, "Doctor/PostAppointment", null, objAppointment).Result;
                    if (result != null)
                    {
                        objAppointment = result.GenericModel;
                    }
                    if (objAppointment == null)
                    {
                        return Json(result);
                    }
                    else
                    {
                        return Json(result);
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
    }
}