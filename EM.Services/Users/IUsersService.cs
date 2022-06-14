#region using
using EM.Entity;
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
        /// <returns></returns>
        User GetByEmail(string email);

        /// <summary>
        /// Get id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetById(int id);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetAllUser();

        /// <summary>
        /// Get users list for admin page
        /// </summary>
        /// <returns></returns>
        IEnumerable<User> GetUserList();

        /// <summary>
        /// Method for register user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User Register(User user);

        /// <summary>
        /// Method for add new user from admin side
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User AddNewUser(User user);

        /// <summary>
        /// Method for set user password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User SetUserPassword(User user);

        /// <summary>
        /// Method for verify logged in user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User VerifyLogin(User user);

        /// <summary>
        /// Method for update existing user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User UpdateDetails(User user);

        /// <summary>
        /// Method for delete user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User DeleteDetails(int id);

        public string GenerateJWT(string Email, string Role);
    }
}
