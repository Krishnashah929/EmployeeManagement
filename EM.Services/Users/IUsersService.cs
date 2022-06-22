#region using
using EM.Entity;
using EM.Models;
using System.Collections.Generic;
#endregion

namespace EM.Services
{
    /// <summary>
    /// Interface for User services for crud operations.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Get email
        /// </summary>
        /// <param name="email"></param>
        /// <returns> returns user's email in string </returns>
        User GetByEmail(string email);

        /// <summary>
        /// Get id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> returns user's id </returns>
        User GetById(int id);
     
        /// <summary>
        /// Get doctor by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> returns doctor's id </returns>
        Doctor GetByDoctorId(int id);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetAllUser();

        /// <summary>
        /// Get all doctors
        /// </summary>
        /// <returns></returns>
        IEnumerable<Doctor> GetAllDoctors();

        /// <summary>
        /// Method for register user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Register Model</returns>
        User Register(RegisterModel objRegisterModel);

        /// <summary>
        /// Method for add new user from admin side
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User Model</returns>
        User AddNewUser(User user);

        /// <summary>
        /// Method for set user password
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User Model</returns>
        User SetUserPassword(User user);

        /// <summary>
        /// Method for verify logged in user
        /// </summary>
        /// <param name="objLoginModel"></param>
        /// <returns>User Model</returns>
        User VerifyLogin(LoginModel objLoginModel);

        /// <summary>
        /// Method for update existing user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User Model</returns>
        User UpdateDetails(User user);

        /// <summary>
        /// Method for update existing doctor 
        /// </summary>
        /// <param name="doctor"></param>
        /// <returns>doctor Model</returns>
        Doctor UpdateDoctorDetails(Doctor doctor);

        /// <summary>
        /// Method for delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user's id</returns>
        User DeleteDetails(int id);
    }
}
