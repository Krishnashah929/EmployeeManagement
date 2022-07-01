#region using
using EM.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#endregion

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
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [DisplayName("Last Name")]
        public string Lastname { get; set; }

        /// <summary>
        /// Email Address input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [DisplayName("Email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Defien user role
        /// </summary>
        public string Role { get; set; }
    }
}