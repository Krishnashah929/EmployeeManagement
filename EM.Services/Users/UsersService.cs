#region using
using EM.Common;
using EM.Entity;
using EM.GenericUnitOfWork.Uow;
using EM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using static EM.Common.GlobalEnum;
#endregion

namespace EM.Services
{
    /// <summary>
    /// User services for crud operations
    /// </summary>
    public class UsersService : IUsersService
    {
        #region Fields
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="configuration"></param>
        public UsersService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this._unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        #endregion

        /// <summary>
        /// Method for get all users
        /// </summary>
        /// <returns>All users who is not deleted</returns>
        #region GetAllUser
        public IEnumerable<User> GetAllUser()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<User>();
                List<User> lstUsers = repoList.GetAll().Where(x => x.IsDelete == false).AsNoTracking().ToList();
                if (lstUsers != null)
                {
                    return lstUsers;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for get all doctors
        /// </summary>
        /// <returns>All users who is not deleted</returns>
        #region GetAllDoctors
        public IEnumerable<Doctor> GetAllDoctors()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<Doctor>();
                var userList = this._unitOfWork.GetRepository<User>();

                //Join operation for fetching user id from user table to doctor table.
                var data = (from d in repoList.GetAll() //d = doctor
                            join u in userList.GetAll() on d.UserId equals u.UserId //u = user
                            select new Doctor()
                            {
                                FirstName = u.FirstName,
                                Lastname = u.Lastname,
                                EmailAddress = u.EmailAddress,
                                DoctorId = d.DoctorId,
                                PhoneNumber = d.PhoneNumber,
                                Pincode = d.Pincode,
                                Address = d.Address,
                                Color = d.Color
                            }).ToList();

                List<Doctor> lstDoctors = data;

                if (lstDoctors != null)
                {
                    return lstDoctors;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return particular user by id</returns>
        #region GetById
        public User GetById(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<User>();
                return repoList.GetByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for get doctor by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return particular doctor by id</returns>
        #region GetByDoctorId
        public Doctor GetByDoctorId(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<Doctor>();
                var userList = this._unitOfWork.GetRepository<User>();
                var specialityList = this._unitOfWork.GetRepository<Speciality>();

                //Join operation for fetching user id from user table to doctor table.
                var data = (from d in repoList.GetAll().Where(x => x.DoctorId == id) //d = doctor
                            join u in userList.GetAll() on d.UserId equals u.UserId //u = user
                            select new Doctor()
                            {
                                FirstName = u.FirstName,
                                Lastname = u.Lastname,
                                EmailAddress = u.EmailAddress,
                                UserId = u.UserId,
                                DoctorId = d.DoctorId,
                                PhoneNumber = d.PhoneNumber,
                                Pincode = d.Pincode,
                                Address = d.Address,
                                CityId = d.CityId,
                                StatesId = d.StatesId,
                                CountryId = d.CountryId,
                                Color = d.Color,
                                CreatedDate = DateTime.Now,
                            }).FirstOrDefault();

                //Get list of particular doctor's speciality
                data.SpecialityId = specialityList.GetAll().Where(x => x.DoctorId == data.DoctorId).Select(x => x.SpecialityId).ToList();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for get Appointment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>return particular Appointment</returns>
        #region GetAppointmentById
        public Appointment GetAppointmentById(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<Appointment>();
                var getAppointment = this.GetAppointments().FirstOrDefault(x => x.AppointmentId == id);

                return getAppointment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>return particular user by email</returns>
        #region GetByEmail
        public User GetByEmail(string email)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<User>();
                return this.GetAllUser().FirstOrDefault(x => x.EmailAddress.ToLower() == email.ToLower());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for register new user
        /// </summary>
        /// <param name="objRegisterModel"></param>
        /// <returns>User model with new user registration</returns>
        #region Register
        public User Register(RegisterModel objRegisterModel)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                User verifyUser = new User();
                //check that new email is already registered or not 
                verifyUser = this.GetAllUser().FirstOrDefault(x => x.EmailAddress == objRegisterModel.EmailAddress);
                if (verifyUser == null)
                {
                    User verifyUsers = new User();
                    if (userRepository != null)
                    {
                        verifyUsers.FirstName = objRegisterModel.FirstName;
                        verifyUsers.Lastname = objRegisterModel.Lastname;
                        verifyUsers.EmailAddress = objRegisterModel.EmailAddress;
                        verifyUsers.Password = string.Empty;
                        verifyUsers.CreatedDate = DateTime.Now;
                        verifyUsers.IsActive = false;
                        //By default new register person will registered as user
                        verifyUsers.Role = (UserRoles.Users).ToString();
                        {
                            verifyUsers.Role = "2";
                        }

                        userRepository.Add(verifyUsers);
                        _unitOfWork.Commit();

                    }
                    return verifyUsers;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for set user password
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User model with modified user details</returns>
        #region SetUserPassword
        public User SetUserPassword(User user)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                User setUserPassword = new User();
                setUserPassword = this.GetAllUser().FirstOrDefault(x => x.UserId == user.UserId);

                setUserPassword.Password = EncryptionDecryption.Encrypt(user.Password.ToString());
                setUserPassword.IsActive = true;
                setUserPassword.ModifiedDate = DateTime.Now;

                _unitOfWork.GetRepository<User>().Update(setUserPassword);
                _unitOfWork.Commit();

                return setUserPassword;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        ///  Method for verify logged in user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Logged in user details</returns>
        #region VerifyLogin
        public User VerifyLogin(LoginModel objLoginModel)
        {
            try
            {
                User verifyLogin = new User();
                var password = EncryptionDecryption.Encrypt(objLoginModel.Password.ToString());
                //Verify Logged in user 
                verifyLogin = this.GetAllUser().FirstOrDefault(x => x.Password == password && x.EmailAddress == objLoginModel.EmailAddress && x.IsActive == true && x.IsDelete == false);
                if (verifyLogin != null)
                {
                    return verifyLogin;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        ///  Method for add new user from admin side
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User model with new user registration</returns>
        #region AddNewUser
        public User AddNewUser(User user)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                User verifyUser = new User();
                //check that new email is already registered or not 
                verifyUser = this.GetAllUser().FirstOrDefault(x => x.EmailAddress == user.EmailAddress);
                if (verifyUser == null)
                {
                    if (userRepository != null)
                    {
                        user.Password = string.Empty;
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = false;
                        int roleId = Convert.ToInt32(user.Role);
                        string roleName = ((UserRoles)roleId).ToString();
                    }
                    userRepository.Add(user);
                    _unitOfWork.Commit();

                    //Add records into the doctor table also id user role is 4(Doctor).
                    if (user.Role == "4")
                    {
                        Doctor doctorlist = new Doctor();
                        var doctorRepository = _unitOfWork.GetRepository<Doctor>();

                        doctorlist.UserId = user.UserId;
                        doctorlist.PhoneNumber = 0;
                        doctorlist.Pincode = 0;
                        doctorlist.CityId = 0;
                        doctorlist.StatesId = 0;
                        doctorlist.CountryId = 0;
                        doctorlist.Address = string.Empty;
                        doctorlist.Color = string.Empty;
                        doctorlist.CreatedBy = 0;
                        doctorlist.CreatedDate = DateTime.Now;
                        doctorRepository.Add(doctorlist);
                        _unitOfWork.Commit();
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for update existing user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User model with modified user details</returns>
        #region UpdateDetails
        public User UpdateDetails(User user)
        {
            try
            {
                if (user != null)
                {
                    var userRepository = _unitOfWork.GetRepository<User>();
                    User updateDetails = new User();
                    updateDetails = this.GetAllUser().FirstOrDefault(x => x.UserId == user.UserId);
                    {
                        if (userRepository != null)
                        {
                            updateDetails.FirstName = user.FirstName;
                            updateDetails.Lastname = user.Lastname;
                            var userEmail = this.GetAllUser().FirstOrDefault(x => x.EmailAddress == user.EmailAddress);
                            if (userEmail == null)
                            {
                                updateDetails.EmailAddress = user.EmailAddress;
                            }
                            updateDetails.Role = user.Role;
                            updateDetails.ModifiedDate = DateTime.Now;
                            _unitOfWork.GetRepository<User>().Update(updateDetails);
                            _unitOfWork.Commit();
                        }
                        return user;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        #region DeleteDetails
        public User DeleteDetails(int id)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                var user = this.GetAllUser().FirstOrDefault(x => x.UserId == id);
                user.IsDelete = true;
                _unitOfWork.GetRepository<User>().Update(user);
                _unitOfWork.Commit();

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for update existing doctor 
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns>User model with modified doctor details</returns>
        #region UpdateDoctorDetails
        public Doctor UpdateDoctorDetails(Doctor doctor)
        {
            try
            {
                _unitOfWork.GetRepository<Doctor>();
                Doctor UpdateDetails = new Doctor();

                var objDoctor = this.GetAllDoctors().FirstOrDefault(x => x.DoctorId == doctor.DoctorId);
                if (objDoctor != null && objDoctor.DoctorId > 0)
                {
                    doctor.ModifiedDate = DateTime.Now;
                    doctor.ModifiedBy = 1;

                    //1. GET ALL FROM DB BASED ON DOCTOR ID
                    var repoSpeciality = _unitOfWork.GetRepository<Speciality>();
                    var lstGetAllSpeciality = repoSpeciality.FindBy(x => x.DoctorId == doctor.DoctorId).ToList();

                    //2. CREATE A LIST which WE NEED TO ADD ON DB , which is new
                    var lstAddSpeciality = new List<Speciality>();
                    foreach (var item in doctor.SpecialityId)
                    {
                        if (lstGetAllSpeciality.Count(x => x.SpecialityId == item) == 0)
                        {
                            lstAddSpeciality.Add(new Speciality()
                            {
                                SpecialityId = item,
                                DoctorId = doctor.DoctorId
                            });
                        }
                    }

                    //3.CREATE A LIST which we need to DELETE FROM DB.
                    var lstRemoveSpeciality = new List<Speciality>();
                    foreach (var item in lstGetAllSpeciality)
                    {
                        if (doctor.SpecialityId.Count(x => x == item.SpecialityId) == 0)
                        {
                            lstRemoveSpeciality.Add(item);
                        }
                    }

                    _unitOfWork.GetRepository<Doctor>().Update(doctor);
                    _unitOfWork.GetRepository<Speciality>().RemoveRange(lstRemoveSpeciality);
                    _unitOfWork.GetRepository<Speciality>().AddRange(lstAddSpeciality);
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return doctor;
        }
        #endregion

        /// <summary>
        /// Method for get all countries
        /// </summary>
        /// <returns>List of countries</returns>
        #region GetCountry
        public IEnumerable<Country> GetCountry()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<Country>();
                List<Country> lsCountries = repoList.GetAll().AsNoTracking().ToList();
                if (lsCountries != null)
                {
                    return lsCountries;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for get all states
        /// </summary>
        /// <returns>List of states</returns>
        #region GetState
        public IEnumerable<State> GetState(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<State>();
                List<State> lsStates = repoList.GetAll().Where(x => x.CountryId == id).AsNoTracking().ToList();
                if (lsStates != null)
                {
                    return lsStates;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for get all cities
        /// </summary>
        /// <returns>List of cities</returns>
        #region GetCity
        public IEnumerable<City> GetCity(int id)
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<City>();
                List<City> lsCities = repoList.GetAll().Where(x => x.StatesId == id).AsNoTracking().ToList();
                if (lsCities != null)
                {
                    return lsCities;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for get all appointments
        /// </summary>
        /// <returns>List of appointments</returns>
        #region GetAppointments
        public IEnumerable<Appointment> GetAppointments()
        {
            try
            {
                var appointmentRepoList = this._unitOfWork.GetRepository<Appointment>();
                var doctorRepoList = this._unitOfWork.GetRepository<Doctor>();

                //Join operation for fetching user id from user table to doctor table.
                var data = (from a in appointmentRepoList.GetAll().AsNoTracking().ToList() //a = appointment
                            join d in doctorRepoList.GetAll() on a.DoctorId equals d.DoctorId //d = doctor
                            let colorValue = Convert.ToInt32(d.Color)
                            select new Appointment()
                            {
                                DoctorId = d.DoctorId,
                                Color = colorValue == Convert.ToInt32(doctorColors.Red) ? "red" :
                                        colorValue == Convert.ToInt32(doctorColors.Blue) ? "blue":
                                        colorValue == Convert.ToInt32(doctorColors.Green) ? "green" : "",//Get color value from enum
                                AppointmentId = a.AppointmentId,
                                FirstName = a.FirstName,
                                LastName = a.LastName,
                                EmailAddress = a.EmailAddress,
                                PhoneNumber = a.PhoneNumber,
                                Diagnosis = a.Diagnosis,
                                Remarks = a.Remarks,
                                StartDateTime = a.StartDateTime,
                                EndDateTime = a.EndDateTime,
                                CreatedDate = DateTime.Now,
                            }).ToList();
                if (data != null)
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Book appointment
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns>Post appointment for user.</returns>
        #region PostAppointment
        public Appointment PostAppointment(Appointment appointment)
        {
            try
            {
                var appoinmentRepository = _unitOfWork.GetRepository<Appointment>();
                Appointment userAppointment = new Appointment();
                {
                    if (appoinmentRepository != null)
                    { 
                        //Checking appointment time respect to doctor id 
                        var appointmentTime = this.GetAppointments().FirstOrDefault(x => x.DoctorId == appointment.DoctorId && x.StartDateTime <= appointment.StartDateTime && x.EndDateTime >= appointment.EndDateTime);
                        if (appointmentTime == null)
                        {
                            userAppointment.StartDateTime = appointment.StartDateTime;
                            userAppointment.EndDateTime = appointment.EndDateTime;
                            userAppointment.DoctorId = appointment.DoctorId;
                            userAppointment.FirstName = appointment.FirstName;
                            userAppointment.LastName = appointment.LastName;
                            userAppointment.EmailAddress = appointment.EmailAddress;
                            userAppointment.PhoneNumber = appointment.PhoneNumber;
                            userAppointment.Diagnosis = appointment.Diagnosis;
                            userAppointment.Remarks = appointment.Remarks;
                            userAppointment.CreatedDate = DateTime.Now;
                            _unitOfWork.GetRepository<Appointment>().Add(userAppointment);
                            _unitOfWork.Commit();

                            //For add records into email table
                            if(userAppointment.AppointmentId > 0)
                            {
                                Email emailList = new Email();
                                var emailRepository = _unitOfWork.GetRepository<Email>();

                                emailList.ToEmail = userAppointment.EmailAddress;
                                emailList.IsSend = true;
                                emailList.EmailBody = userAppointment.Diagnosis;
                                emailList.CreatedBy = 0;
                                emailList.CreatedDate = DateTime.Now;
                                emailRepository.Add(emailList);
                                _unitOfWork.Commit();
                            }
                        }

                        else
                        {
                            return null;
                        }
                    }
                    return userAppointment;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Method for update existing appointment 
        /// </summary>
        /// <param name="objAppointment"></param>
        /// <returns>Appointment model with modified appointment details</returns>
        #region EditAppointment
        public Appointment EditAppointment(Appointment objAppointment)
        {
            try
            {
                if (objAppointment != null)
                {
                    var userRepository = _unitOfWork.GetRepository<Appointment>();
                    Appointment updateAppointment = new Appointment();
                    updateAppointment = this.GetAppointments().FirstOrDefault(x => x.AppointmentId == objAppointment.AppointmentId);
                    {
                        if (userRepository != null)
                        {
                            updateAppointment.FirstName = objAppointment.FirstName;
                            updateAppointment.LastName = objAppointment.LastName;
                            updateAppointment.PhoneNumber = objAppointment.PhoneNumber;
                            updateAppointment.Diagnosis = objAppointment.Diagnosis;
                            updateAppointment.Remarks = objAppointment.Remarks;
                            updateAppointment.DoctorId = objAppointment.DoctorId;
                            updateAppointment.ModifiedDate = DateTime.Now;
                            //Checkinh that this email is already registered or not
                            var userEmail = this.GetAllUser().FirstOrDefault(x => x.EmailAddress == objAppointment.EmailAddress);
                            if (userEmail == null)
                            {
                                updateAppointment.EmailAddress = objAppointment.EmailAddress;
                            }
                            //Checking appointment time with respect to doctor id
                            var appointmentTime = this.GetAppointments().FirstOrDefault(x => x.DoctorId == objAppointment.DoctorId &&  x.StartDateTime == objAppointment.StartDateTime && x.EndDateTime == objAppointment.EndDateTime);
                            if (appointmentTime == null && objAppointment.StartDateTime >= updateAppointment.StartDateTime && objAppointment.EndDateTime <= updateAppointment.EndDateTime) 
                            {
                                updateAppointment.StartDateTime = objAppointment.StartDateTime;
                                updateAppointment.EndDateTime = objAppointment.EndDateTime;
                            }
                            _unitOfWork.GetRepository<Appointment>().Update(updateAppointment);
                            _unitOfWork.Commit();
                            return updateAppointment;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for delete appointment
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Delete record permanently</returns>
        #region DeleteAppointment
        public Appointment DeleteAppointment(int id)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<Appointment>();
                var deleteAppointment = this.GetAppointments().FirstOrDefault(x => x.AppointmentId == id);
                _unitOfWork.GetRepository<Appointment>().HardDelete(deleteAppointment);
                _unitOfWork.Commit();

                return deleteAppointment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// Method for event date for drag and drop
        /// </summary>
        /// <param name="objDragAndDrop"></param>
        /// <returns>List of events</returns>
        #region DragAndDrop
        public Appointment DragAndDrop(DragAndDrop objDragAndDrop)
        {
            try
            {
                var eventRepository = _unitOfWork.GetRepository<Appointment>();
                Appointment updateEvent = new Appointment();
                if (objDragAndDrop != null)
                {
                    {
                        if (eventRepository != null)
                        {
                            var userEvent = this.GetAppointments().FirstOrDefault(x => x.AppointmentId == objDragAndDrop.AppointmentId);
                            //Checking appointment time with respect to doctor id
                            var appointmentTime = this.GetAppointments().FirstOrDefault(x => x.DoctorId == objDragAndDrop.DoctorId && x.StartDateTime == objDragAndDrop.StartDateTime && x.EndDateTime == objDragAndDrop.EndDateTime);
                            if (appointmentTime == null)
                            {
                                updateEvent.AppointmentId = objDragAndDrop.AppointmentId;
                                updateEvent.DoctorId = objDragAndDrop.DoctorId;
                                updateEvent.StartDateTime = objDragAndDrop.StartDateTime;
                                updateEvent.EndDateTime = objDragAndDrop.EndDateTime;
                                updateEvent.FirstName = userEvent.FirstName;
                                updateEvent.LastName = userEvent.LastName;
                                updateEvent.EmailAddress = userEvent.EmailAddress;
                                updateEvent.PhoneNumber = userEvent.PhoneNumber;
                                updateEvent.Diagnosis = userEvent.Diagnosis;
                                updateEvent.Remarks = userEvent.Remarks;
                                updateEvent.ModifiedDate = DateTime.Now;
                                _unitOfWork.GetRepository<Appointment>().Update(updateEvent);
                                _unitOfWork.Commit();
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                return updateEvent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}