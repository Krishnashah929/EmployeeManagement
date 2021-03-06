#region using
using ElmahCore;
using EM.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
#endregion

/// <summary>
/// Home controller for users.
/// </summary>
namespace EM.Web.Controllers
{
    /// <summary>
    /// Calling cache from startup.cs
    /// </summary>
    [ResponseCache(CacheProfileName = "Default0")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Main Dashboard when user is not logged in.
        /// </summary>
        #region Dashboard
        [Authorize(Roles = "Admin, Users,Receptionist")]
        public IActionResult Dashboard()
        {
            HttpContext.RaiseError(new InvalidOperationException("Dashboard"));
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
