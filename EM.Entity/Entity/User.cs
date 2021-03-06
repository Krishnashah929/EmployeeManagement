#region using
using EM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

#nullable disable

namespace EM.Entity
{
    /// <summary>
    /// Main user entity 
    /// </summary>
    public partial class User
    {

        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }
        /// <summary>
        /// Get userId.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// FirstName input feild.
        /// </summary>
        [DisplayName ("First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName input feild.
        /// </summary>
        [DisplayName("Last Name")]
        public string Lastname { get; set; }

        /// <summary>
        /// Email Address input feild.
        /// </summary>
        [MaxLength(50)]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Password input feild.
        /// </summary>
        [DataType(DataType.Password)]
        [StringLength(10, ErrorMessage = CommonValidations.RequiredLengthErrorMsg, MinimumLength = 8)]
        [DisplayName("Password")]
        public string Password { get; set; }

        /// <summary>
        /// RetypePassword it is for confirm user's password.
        /// This value is not stored into the database.
        /// </summary>
        [NotMapped]
        [Compare("Password", ErrorMessage = CommonValidations.ComparePasswordMsg)]
        [DataType(DataType.Password)]
        [DisplayName("Reenter Password")]
        public string RetypePassword { get; set; }

        /// <summary>
        /// Isactive feild for users.
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// IsDeleted feild when you have to delete user.
        /// </summary>
        [DisplayName("Delete")]
        public bool IsDelete { get; set; }

        /// <summary>
        /// IsBlocked feild when you have to delete user.
        /// </summary>
        [DisplayName("Blocked")]
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Created by field for first time of creation of user.
        /// </summary>
        [DisplayName("Created By")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Created date field for first time of user create account date.
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// ModifiedBy field for modiffy details of user.
        /// </summary>
        [DisplayName("Modified By")]
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// ModifiedDate field for user modify their details.
        /// </summary>
        [DisplayName("Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Get or set userrole table
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; }
        
        /// <summary>
        /// Get or set user role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        ///this line for the mailing services
        /// </summary>
        public string ResetPasswordCode { get; set; }

        /// <summary>
        /// For created by name
        /// </summary>
        [NotMapped]
        public string CreateByName { get; set; }
         
        /// <summary>
        /// For modified by name
        /// </summary>
        [NotMapped]
        public string ModifiedByName { get; set; }

        /// <summary>
        /// For getting role in string format for data table
        /// </summary>
        [NotMapped]
        public string RoleName { get; set; }
    }
}