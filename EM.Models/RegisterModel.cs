﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Claims;
using EM.Common;

namespace EM.Models
{
    public class RegisterModel
    {
        /// <summary>
        /// firstname input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        //[StringLength(8, ErrorMessage = CommonValidations.RequiredLengthErrorMsg, MinimumLength = 6)]
        [DisplayName("first name")]
        public string FirstName { get; set; }

        /// <summary>
        /// lastname input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        //[StringLength(8, ErrorMessage = CommonValidations.RequiredLengthErrorMsg, MinimumLength = 6)]
        [DisplayName("last name")]
        public string Lastname { get; set; }

        /// <summary>
        /// Email Address input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [MaxLength(50)]
        [DisplayName ("Email")]
        public string EmailAddress { get; set; }

       
        ///// <summary>
        ///// UserRole input feild.
        ///// </summary>
        //public string Role { get; set; }

    }
}