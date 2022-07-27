#region Using
using EM.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Models
{
    /// <summary>
    /// Model for send email
    /// </summary>
    public partial class SendMail
    {
        /// <summary>
        /// Get Customer Emaill
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [DisplayName("Customer Email")]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Get ExpiryDate
        /// </summary>
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        [DisplayName("Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [NotMapped]
        public int FormId { get; set; }
    }
}
