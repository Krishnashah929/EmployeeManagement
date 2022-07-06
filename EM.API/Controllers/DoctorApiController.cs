#region using
using EM.API.Helpers;
using EM.Models;
using EM.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Linq.Dynamic.Core;
using EM.Entity;
using EM.Common;
using Microsoft.AspNetCore.Authorization;
#endregion

namespace EM.API.Controllers
{
    /// <summary>
    /// Api controller for doctor side pages 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //For jwt authorization
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class DoctorApiController : ControllerBase
    {
        /// <summary>
        /// Generate fields for IUsersService 
        /// </summary>
        #region Fields
        private IUsersService _userService;
        public object HttpCacheability { get; private set; }
        #endregion

        /// <summary>
        /// Constructors for DoctorApiController.
        /// </summary>
        /// <param name="userService"></param>
        #region Constructors
        [Obsolete]
        public DoctorApiController(IUsersService userService)
        {
            _userService = userService;
        }
        #endregion

        //////////////////////// Doctor' s method ////////////////////////

        /// <summary>
        /// Getting doctors list
        /// </summary>
        /// <param name="jqueryDatatableParam"></param>
        /// <returns>all list of doctors</returns>
        #region GetDoctorList
        [HttpPost("GetDoctorList")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel GetDoctorList(JqueryDatatableParam jqueryDatatableParam)
        {
            try
            {
                int totalRecord = 0;
                int filterRecord = 0;

                var data = _userService.GetAllDoctors();
                if (data != null)
                {
                    // get total count of records 
                    totalRecord = data.Count();

                    // search data when search value found
                    if (!string.IsNullOrEmpty(jqueryDatatableParam.searchValue))
                    {
                        data = data.Where(x =>
                          x.FirstName.ToLower().Contains(jqueryDatatableParam.searchValue.ToLower())
                          || x.Lastname.ToLower().Contains(jqueryDatatableParam.searchValue.ToLower())
                          || x.EmailAddress.ToLower().Contains(jqueryDatatableParam.searchValue.ToLower())
                        );
                    }
                    // get total count of records after search 
                    filterRecord = data.Count();

                    //sort data
                    if (!string.IsNullOrEmpty(jqueryDatatableParam.sortColumn) && !string.IsNullOrEmpty(jqueryDatatableParam.sortColumnDirection))
                        data = data.AsQueryable().OrderBy(jqueryDatatableParam.sortColumn + " " + jqueryDatatableParam.sortColumnDirection);

                    //pagination
                    var empList = data.Skip(jqueryDatatableParam.skip).Take(jqueryDatatableParam.pageSize).ToList();
                    //getting value from common helper.
                    return CommonHelper.GetResponseDataTable(jqueryDatatableParam.draw, totalRecord, filterRecord, empList);
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
        /// Get Doctors 
        /// </summary>
        /// <returns>List of doctors</returns>
        #region GetDoctorForAppointment
        [HttpGet("GetDoctor")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel GetDoctor()
        {
            try
            {
                var getDoctors = _userService.GetSortedDoctors();
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.OK, "", "", getDoctors);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "");
            }
        }
        #endregion

        /// <summary>
        /// Get method edit doctor details 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>open edit model</returns>
        #region EditDoctorModel(GET)
        [HttpGet("EditDoctorModel/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel EditDoctorModel(int id)
        {
            try
            {
                var userDetails = _userService.GetByDoctorId(id);
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
        /// Post method for edit details
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns>edit existing doctor details from admin side</returns>
        #region EditDoctorPost
        [HttpPut("EditDoctor")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel EditDoctor(Doctor objDoctor)
        {
            try
            {
                if (objDoctor != null)
                {
                    var editDoctor = _userService.UpdateDoctorDetails(objDoctor);
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.UpdateDoctorsDetails, editDoctor);
                }
                else
                {
                    return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg);
                }
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.InvalidMsg);
            }
        }
        #endregion

        /// <summary>
        /// Get Country 
        /// </summary>
        /// <returns>List of countries</returns>
        #region GetCountry
        [HttpGet("GetCountry")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel GetCountry()
        {
            try
            {
                var getCountry = _userService.GetCountry();
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.OK, "", "", getCountry);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "");
            }
        }
        #endregion

        /// <summary>
        /// GetState 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of states</returns>
        #region GetState
        [HttpGet("GetState/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel GetState(int id)
        {
            try
            {
                var getState = _userService.GetState(id);
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.OK, "", "", getState);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "");
            }
        }
        #endregion

        /// <summary>
        /// GetCity
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of cities</returns>
        #region GetCity
        [HttpGet("GetCity/{id}")]
        [Authorize(Roles = "Admin")]
        public ApiResponseModel GetCity(int id)
        {
            try
            {
                var getCity = _userService.GetCity(id);
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.OK, "", "", getCity);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "");
            }
        }
        #endregion

        //////////////////////// TimeSheet' s method ////////////////////////
        
        /// <summary>
        /// Get Appointments 
        /// </summary>
        /// <returns>List of appointments</returns>
        #region GetAppointments
        [HttpGet("GetAppointments")]
        [Authorize(Roles = "Admin , Receptionist")]
        public ApiResponseModel GetAppointments()
        {
            try
            {
                var getAppointments = _userService.GetAppointments();
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.OK, "", "", getAppointments);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "");
            }
        }
        #endregion

        /// <summary>
        ///  Post Appontment .
        /// </summary>
        /// <param name="objAppointment"></param>
        /// <returns>Add Appointment for user. </returns>
        #region PostAppointment
        [HttpPut("PostAppointment")]
        [Authorize(Roles = "Admin , Receptionist")]
        public ApiResponseModel PostAppointment(Appointment objAppointment)
        {
            try
            {
                if (objAppointment != null)
                {
                    var postAppointment = _userService.PostAppointment(objAppointment);
                    if(postAppointment == null)
                    {
                        //getting value from common helper.
                        return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.DoctorUnavailableMsg, postAppointment);
                    }
                    else
                    {
                        return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.BookedAppointment, postAppointment);
                    }
                }
                else
                {
                    return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg);
                }
            }
            catch (Exception)
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "");
            }
        }
        #endregion

        /// <summary>
        /// Method for getting Appointment model for add and edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Add/edit Appointment model</returns>
        #region EditAppointmentGet(GET)
        [HttpGet("EditAppointmentModel/{id}")]
        [Authorize(Roles = "Admin , Receptionist")]
        public ApiResponseModel EditAppointmentModel(int id)
        {
            try
            {
                var getAppointment = _userService.GetAppointmentById(id);
                if (getAppointment != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", getAppointment);
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
        /// Post method for edit appointments
        /// </summary>
        /// <param name="user"></param>
        /// <returns>edit existing appointment</returns>
        #region EditAppointment(POST)
        [HttpPut("EditAppointment")]
        [Authorize(Roles = "Admin , Receptionist")]
        public ApiResponseModel EditAppointment(Appointment objAppointment)
        {
            try
            {
                if (objAppointment != null)
                {
                    var editAppointment = _userService.EditAppointment(objAppointment);
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.UpdateAppointment, editAppointment);
                }
                else
                {
                    return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg);
                }
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, "");
            }
        }
        #endregion

        /// <summary>
        ///   get delete Appointment 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>open delete appointment model</returns>
        #region DeleteAppointmentModel(GET)
        [HttpGet("DeleteAppointmentModel/{id}")]
        [Authorize(Roles = "Admin , Receptionist")]
        public ApiResponseModel DeleteAppointmentModel(int id)
        {
            try
            {
                var deleteAppointment = _userService.GetAppointmentById(id);
                if (deleteAppointment != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, "", deleteAppointment);
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
        /// Post method for delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>delete user details from admin side</returns>
        #region DeleteAppointments(POST)
        [HttpDelete("DeleteAppointments/{id}")]
        [Authorize(Roles = "Admin , Receptionist")]
        public ApiResponseModel DeleteAppointments(int id)
        {
            try
            {
                var deleteAppointment = _userService.DeleteAppointment(id);
                //getting value from common helper.
                return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.DeleteUserDetails, deleteAppointment);
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, CommonValidations.RecordNotExistsMsg, "");
            }
        }
        #endregion

        /// <summary>
        /// Method for event date for drag and drop
        /// </summary>
        /// <param name="objDragAndDrop"></param>
        /// <returns>List of events</returns>
        #region DragAndDrop(GET)
        [HttpPost("DragAndDrop")]
        [Authorize(Roles = "Admin , Receptionist")]
        public ApiResponseModel DragAndDrop(DragAndDrop objDragAndDrop)
        {
            try
            {
                var getAppointment = _userService.DragAndDrop(objDragAndDrop);
                if (getAppointment != null)
                {
                    //getting value from common helper.
                    return CommonHelper.GetResponse(HttpStatusCode.OK, CommonValidations.UpdateAppointment, getAppointment);
                }
                else if (getAppointment == null)
                {
                    return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.NotDropEventMsg, "");
                }
                return CommonHelper.GetResponse(HttpStatusCode.BadRequest, CommonValidations.RecordExistsMsg, "");
            }
            catch
            {
                return CommonHelper.GetResponse(HttpStatusCode.InternalServerError, "", "");
            }
        }
        #endregion
    }
}