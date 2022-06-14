﻿#region using
using EM.Common;
using EM.Entity;
using EM.GenericUnitOfWork.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        /// <returns></returns>
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
        /// Get users list for admin page
        /// </summary>
        /// <returns></returns>
        #region GetUserList
        public IEnumerable<User> GetUserList()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<User>();
                List<User> lstUsers = repoList.GetAll().Where(x => x.IsDelete == false && x.Role == "1").AsNoTracking().ToList();

                return lstUsers;
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
        /// <returns></returns>
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
        /// Method for get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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
        /// <param name="user"></param>
        /// <returns></returns>
        #region Register
        public User Register(User user)
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
                        user.Role = "2";

                        userRepository.Add(user);
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
        /// Method for set user password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        #region VerifyLogin
        public User VerifyLogin(User user)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                User verifyLogin = new User();
                var password = EncryptionDecryption.Encrypt(user.Password.ToString());
                verifyLogin = this.GetAllUser().FirstOrDefault(x => x.Password == password && x.EmailAddress == user.EmailAddress && x.IsActive == true && x.IsDelete == false);
                //get jwt token 
               
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
        /// <returns></returns>
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
                        if (user.Role == "Admin")
                        {
                            user.Role = "1";
                        }
                        else if (user.Role == "User")
                        {
                            user.Role = "2";
                        }
                    }

                    userRepository.Add(user);
                    _unitOfWork.Commit();
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
        /// <returns></returns>
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
        /// For generate jwt token of varified login user.
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Role"></param>
        /// <returns></returns>
        #region GenerateJwt
        public string GenerateJWT(string Email, string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //claim is used to add identity to JWT token
            var claims = new[] {
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Email, Email),
             new Claim(ClaimTypes.Role,Role),
             new Claim("Date", DateTime.Now.ToString()),
        };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Audiance"],
              claims,    //null original value
              expires: DateTime.Now.AddMinutes(120),

              //notBefore:
              signingCredentials: credentials);

            string Data = new JwtSecurityTokenHandler().WriteToken(token); //return access token 
            return Data;
        }
        #endregion
    }
}

