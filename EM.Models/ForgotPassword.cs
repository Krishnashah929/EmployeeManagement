#region using
using EM.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Models
{
    /// <summary>
    /// Forgot model class
    /// </summary>
    public class ForgotPassword
    {
        /// <summary>
        /// Password input feild.
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [DataType(DataType.Password)]
        [StringLength(10, ErrorMessage = CommonValidations.RequiredLengthErrorMsg, MinimumLength = 8)]
        [DisplayName("Password")]
        public string Password { get; set; }

        /// <summary>
        /// RetypePassword it is for confirm user's password.
        /// This value is not stored into the database.
        /// </summary>
        [NotMapped]
        [Required(ErrorMessage = CommonValidations.RetypePasswordMesg)]
        [Compare("Password", ErrorMessage = CommonValidations.ComparePasswordMsg)]
        [DataType(DataType.Password)]
        [DisplayName("Reenter Passwird")]
        public string RetypePassword { get; set; }

        //this line for the mailing services//
        public string ResetPasswordCode { get; set; }
        public string ResetCode { get; set; }
    }
}
