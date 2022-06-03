using EM.Entity;
using System.Collections.Generic;

namespace EM.Services
{
    /// <summary>
    /// Interface for User services for crud operations.
    /// </summary>
    public interface IUsersService
    {
        User GetByEmail(string email);
        User GetById(int id);
        IEnumerable<User> GetAllUser();
        User Register(User user);
        User SetUserPassword(User user);
        User VerifyLogin(User user);
        User UpdateDetails(User user);
        User DeleteDetails(User user);
    }
}
