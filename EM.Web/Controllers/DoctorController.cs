#region Using
using ElmahCore;
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
    /// <summary>
    /// Calling cache from startup.cs
    /// </summary>
    [ResponseCache(CacheProfileName = "Default0")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult TimeSheets()
        {
            HttpContext.RaiseError(new InvalidOperationException("TimeSheets"));
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
        /// Get index of doctor's page
        /// </summary>
        /// <returns>MAin doctor's index page</returns>
        #region GetDoctors
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDoctors()
        {
            HttpContext.RaiseError(new InvalidOperationException("GetDoctors"));
            try
            {
                return View();
            }
            catch(Exception)
            {
                return View("Error");
            }
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
            HttpContext.RaiseError(new InvalidOperationException("GetDoctorsList"));
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
            HttpContext.RaiseError(new InvalidOperationException("GetDoctor"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<Doctor>();
                result = ApiRequest<Doctor>(RequestTypes.Get, "DoctorApi/GetDoctor").Result;
                return new JsonResult(result.GenericList);
            }
            catch(Exception)
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Method for edit doctor details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Edit Doctor model</returns>
        #region EditDoctorsModelGet
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditDoctorsModel(int? id)
        {
            HttpContext.RaiseError(new InvalidOperationException("EditDoctorsModel"));
            Doctor objDoctor = new Doctor();
            try
            {
                if (id > 0)
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
                    objDoctor.CityId = result.GenericModel.CityId;
                    objDoctor.StatesId = result.GenericModel.StatesId;
                    objDoctor.CountryId = result.GenericModel.CountryId;
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
        #region EditDoctorPost
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditDoctor(Doctor objDoctor)
        {
            HttpContext.RaiseError(new InvalidOperationException("EditDoctor"));
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
            HttpContext.RaiseError(new InvalidOperationException("GetCountry"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<Country>();
                result = ApiRequest<Country>(RequestTypes.Get, "DoctorApi/GetCountry").Result;
                return new JsonResult(result.GenericList);
            }
            catch (Exception)
            {
                return null;
            }
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
            HttpContext.RaiseError(new InvalidOperationException("GetState"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<State>();
                result = ApiRequest<State>(RequestTypes.Get, "DoctorApi/GetState/" + id).Result;
                return new JsonResult(result.GenericList);
            }
            catch (Exception)
            {
                return null;
            }
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
            HttpContext.RaiseError(new InvalidOperationException("GetCity"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<City>();
                result = ApiRequest<City>(RequestTypes.Get, "DoctorApi/GetCity/" + id).Result;
                return new JsonResult(result.GenericList);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Get Appointments
        /// </summary>
        /// <returns>List of appointments</returns>
        #region GetAppointments
        [HttpGet]
        [Authorize(Roles = "Admin , Receptionist")]
        public JsonResult GetAppointments()
        {
            HttpContext.RaiseError(new InvalidOperationException("TimeSheets"));
            //Calling BaseController.
            var result = new ApiGenericModel<Appointment>();
            result = ApiRequest<Appointment>(RequestTypes.Get, "DoctorApi/GetAppointments").Result;
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
        [Authorize(Roles = "Admin , Receptionist")]
        public IActionResult PostAppointment(Appointment objAppointment)
        {
            HttpContext.RaiseError(new InvalidOperationException("PostAppointment"));
            try
            {
                var result = new ApiGenericModel<Appointment>();
                if (objAppointment.AppointmentId == 0)
                {
                    //For add appointment.
                    result = ApiRequest<Appointment>(RequestTypes.Put, "DoctorApi/PostAppointment", null, objAppointment).Result;
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
                //For update existing appointment.
                else
                {
                    result = ApiRequest<Appointment>(RequestTypes.Put, "DoctorApi/EditAppointment", null, objAppointment).Result;
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
        }
        #endregion

        /// <summary>
        /// Method for getting Appointment model for add and edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Add/edit Appointment model</returns>
        #region EditAppointmentGet
        [HttpGet]
        [Authorize(Roles = "Admin , Receptionist")]
        public IActionResult EditAppointmentModel(int? id)
        {
            HttpContext.RaiseError(new InvalidOperationException("EditAppointmentModel"));
            Appointment objAppointment = new Appointment();
            try
            {
                if (id > 0)
                {
                    //Calling BaseController.
                    var result = new ApiGenericModel<Appointment>();
                    result = ApiRequest<Appointment>(RequestTypes.Get, "DoctorApi/EditAppointmentModel/" + id).Result;

                    objAppointment.AppointmentId = result.GenericModel.AppointmentId;
                    objAppointment.FirstName = result.GenericModel.FirstName;
                    objAppointment.LastName = result.GenericModel.LastName;
                    objAppointment.EmailAddress = result.GenericModel.EmailAddress;
                    objAppointment.PhoneNumber = result.GenericModel.PhoneNumber;
                    objAppointment.DoctorId = result.GenericModel.DoctorId;
                    objAppointment.Diagnosis = result.GenericModel.Diagnosis;
                    objAppointment.Remarks = result.GenericModel.Remarks;
                    objAppointment.StartDateTime = result.GenericModel.StartDateTime;
                    objAppointment.EndDateTime = result.GenericModel.EndDateTime;
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return new JsonResult(objAppointment);
        }
        #endregion

        /// <summary>
        /// Method for getting delete appointment model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete appointment model</returns>
        #region DeleteAppointmentGet
        [HttpGet]
        [Authorize(Roles = "Admin , Receptionist")]
        public IActionResult DeleteAppointmentModel(int id)
        {
            HttpContext.RaiseError(new InvalidOperationException("DeleteAppointmentModel"));
            try
            {
                //Calling BaseController.
                var result = new ApiGenericModel<Appointment>();
                result = ApiRequest<Appointment>(RequestTypes.Get, "DoctorApi/DeleteAppointmentModel/" + id).Result;
                if (result != null)
                {
                    return new JsonResult(result.GenericModel);
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
            return null;
        }
        #endregion

        /// <summary>
        /// Post method for delete appointment 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Delete appointment</returns>
        #region DeleteAppointmentPost
        [HttpPost]
        [Authorize(Roles = "Admin , Receptionist")]
        public IActionResult DeleteAppointments(Appointment model)
        {
            HttpContext.RaiseError(new InvalidOperationException("DeleteAppointments"));
            try
            {
                ////Calling BaseController.
                var result = new ApiGenericModel<Appointment>();
                result = ApiRequest<Appointment>(RequestTypes.Delete, "DoctorApi/DeleteAppointments/" + model.AppointmentId).Result;
                return Json(result);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        /// <summary>
        /// Method for event date for drag and drop
        /// </summary>
        /// <param name="objDragAndDrop"></param>
        /// <returns>List of events</returns>
        #region DragAndDrop
        [HttpPost]
        [Authorize(Roles = "Admin , Receptionist")]
        public IActionResult DragAndDrop(DragAndDrop objDragAndDrop)
        {
            HttpContext.RaiseError(new InvalidOperationException("DragAndDrop"));
            try
            {
                if (objDragAndDrop != null)
                {
                    //Calling BaseController.
                    var result = new ApiGenericModel<DragAndDrop>();
                    result = ApiRequest<DragAndDrop>(RequestTypes.Post, "DoctorApi/DragAndDrop", null, objDragAndDrop).Result;
                    return Json(result);
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