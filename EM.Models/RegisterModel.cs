using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Claims;
using EM.Common;

namespace EM.Models
{
    /// <summary>
    /// Register model class
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// UserId field.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// firstname input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [DisplayName("first name")]
        public string FirstName { get; set; }

        /// <summary>
        /// lastname input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [DisplayName("last name")]
        public string Lastname { get; set; }

        /// <summary>
        /// Email Address input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [MaxLength(50)]
        [DisplayName ("Email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Defien user role
        /// </summary>
        [Required(ErrorMessage = CommonValidations.SelectRequiredErrorMsg)]
        [DisplayName("Roles")]
        public string RoleId { get; set; }
    }
}
