#region using
using EM.Common;
using EM.Entity;
using EM.GenericUnitOfWork.Uow;
using EM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
                //return this.GetAllUser().FirstOrDefault(x => x.UserId == id);
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
                                CityID = d.CityID,
                                StateID = d.StateID,
                                CountryID = d.CountryID,
                                Color = d.Color,
                            }).FirstOrDefault();

                //Get list of particular doctor's speciality
                data.SpecialityId = specialityList.GetAll().Where(x => x.DoctorId == data.DoctorId).Select(x=> x.SpecialityId).ToList();

                return data;
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
                        doctorlist.CityID = 0;
                        doctorlist.StateID = 0;
                        doctorlist.CountryID = 0;
                        doctorlist.Address = string.Empty;
                        doctorlist.Color = string.Empty;
                        doctorlist.CreatedBy = 0;
                        doctorlist.CreatedDate = DateTime.Now;
                        doctorRepository.Add(doctorlist);
                        _unitOfWork.Commit();

                        //Add record in speciality table as well.
                        Speciality specialitylist = new Speciality();
                        var specialityRepository = _unitOfWork.GetRepository<Speciality>();

                        specialitylist.DoctorId = doctorlist.DoctorId;
                        specialitylist.SpecialityId = 1;
                        specialityRepository.Add(specialitylist);
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
                    User UpdateDetails = new User();
                    UpdateDetails = this.GetAllUser().FirstOrDefault(x => x.UserId == user.UserId);
                    {
                        if (userRepository != null)
                        {
                            UpdateDetails.FirstName = user.FirstName;
                            UpdateDetails.Lastname = user.Lastname;
                            var userEmail = this.GetAllUser().FirstOrDefault(x => x.EmailAddress == user.EmailAddress);
                            if (userEmail == null)
                            {
                                UpdateDetails.EmailAddress = user.EmailAddress;
                            }
                            UpdateDetails.Role = user.Role;
                            UpdateDetails.ModifiedDate = DateTime.Now;
                            _unitOfWork.GetRepository<User>().Update(UpdateDetails);
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
                if (doctor != null)
                {
                    var userRepository = _unitOfWork.GetRepository<Doctor>();
                    Doctor UpdateDetails = new Doctor();
                    UpdateDetails = this.GetAllDoctors().FirstOrDefault(x => x.DoctorId == doctor.DoctorId);
                    {
                        if (userRepository != null)
                        {
                            var userList = this._unitOfWork.GetRepository<User>();

                            var spec = this._unitOfWork.GetRepository<Speciality>(); //spec variable is for speciality
                            //First delete existing specialities
                            spec.HardDelete().Where(x => x.DoctorId == doctor.DoctorId);

                            //Now add updated specialities
                            if(doctor != null && doctor.SpecialityId.Count > 0)
                            {
                                foreach (var item in doctor.SpecialityId)
                                {
                                    Speciality speciality = new Speciality();
                                    speciality.DoctorId = doctor.DoctorId;
                                    speciality.SpecialityId = item;

                                    spec.Add(speciality);
                                }
                            }
                            _unitOfWork.Commit();

                            //Join operation for fetching user id from user table to doctor table.
                            var data = (from d in userRepository.GetAll().Where(x => x.DoctorId == doctor.DoctorId) //d = doctor
                                        join u in userList.GetAll() on d.UserId equals u.UserId //u = user
                                        select new Doctor()
                                        {
                                            UserId = u.UserId,

                                        }).FirstOrDefault();
                            var userId = data.UserId;
                            UpdateDetails.UserId = userId;
                            UpdateDetails.PhoneNumber = doctor.PhoneNumber;
                            UpdateDetails.Pincode = doctor.Pincode;
                            UpdateDetails.Address = doctor.Address;
                            UpdateDetails.CityID = doctor.CityID;
                            UpdateDetails.StateID = doctor.StateID;
                            UpdateDetails.CountryID = doctor.CountryID;
                            UpdateDetails.Color = doctor.Color;
                            UpdateDetails.ModifiedDate = DateTime.Now;

                            _unitOfWork.GetRepository<Doctor>().Update(UpdateDetails);
                            _unitOfWork.Commit();
                        }
                        return doctor;
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
    }
}