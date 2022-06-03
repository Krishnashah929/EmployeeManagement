using EM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EM.Models
{
    /// <summary>
    /// For edit list of users
    /// </summary>
    public class UpdateDetails
    {
        /// <summary>
        /// UserId field.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// FirstName input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        //[StringLength(8, ErrorMessage = CommonValidations.RequiredLengthErrorMsg, MinimumLength = 6)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        //[StringLength(8, ErrorMessage = CommonValidations.RequiredLengthErrorMsg, MinimumLength = 6)]
        [DisplayName("Last Name")]
        public string Lastname { get; set; }

        /// <summary>
        /// Email Address input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        //[StringLength(8, ErrorMessage = CommonValidations.RequiredLengthMailErrorMsg)]
        //[MinLength(5, ErrorMessage = CommonValidations.RequiredLengthMailErrorMsg)]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }
    }
}
