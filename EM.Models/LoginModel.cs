#region using
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using EM.Common;
#endregion

namespace EM.Models
{
    /// <summary>
    /// Login model class
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Email Address input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [MaxLength(50)]
        [DisplayName ("Email")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Password input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [DataType(DataType.Password)]
        [StringLength(10, ErrorMessage = CommonValidations.RequiredLengthErrorMsg, MinimumLength = 8)]
        [DisplayName("Password")]
        public string Password { get; set; }

        ///// <summary>
        ///// UserRole input feild.
        ///// </summary>
        public string Role { get; set; }

    }
}
