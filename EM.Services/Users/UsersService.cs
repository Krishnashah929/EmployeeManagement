using EM.Common;
using EM.Entity;
using EM.GenericUnitOfWork.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The Unit Of Work.</param>
        public UsersService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        #endregion

        public IEnumerable<User> GetAllUser()
        {
            return this.GetAll();
        }
        private List<User> GetAll()
        {
            try
            {
                var repoList = this._unitOfWork.GetRepository<User>();
                List<User> lstUsers = repoList.GetAll().AsNoTracking().ToList();

                return lstUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public User GetById(int id)
        {
            try
            {
                return this.GetAll().FirstOrDefault(x => x.UserId == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public User GetByEmail(string email)
        {
            try
            { 
                return this.GetAll().FirstOrDefault(x => x.EmailAddress.ToLower() == email.ToLower());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public User Register(User user)
        {
            try
            {
                if (user != null)
                {
                    var userRepository = _unitOfWork.GetRepository<User>();
                    if (userRepository != null)
                    {
                        user.Password = string.Empty;
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = false;
                        user.Role = "1";

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
        public User SetUserPassword(User user)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                User setUserPassword = new User();
                setUserPassword = this.GetAll().FirstOrDefault(x => x.UserId == user.UserId);

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

        public User VerifyLogin(User user)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<User>();
                User verifyLogin = new User();
                var password = EncryptionDecryption.Encrypt(user.Password.ToString());
                verifyLogin = this.GetAll().FirstOrDefault(x => x.Password == password && x.EmailAddress == user.EmailAddress && x.IsActive == true && x.IsDelete == false);
                 
                //_unitOfWork.Commit();

                return verifyLogin;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

