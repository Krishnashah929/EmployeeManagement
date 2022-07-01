#region Using
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace EM.Entity
{
    /// <summary>
    /// Get Email 
    /// </summary>
    public partial class Email
    {
        /// <summary>
        /// Get email of user
        /// </summary>
        [Key]
        public string ToEmail { get; set; }
        /// <summary>
        /// Get email body 
        /// </summary>
        public string EmailBody { get; set; }
        /// <summary>
        /// Get email is send or not
        /// </summary>
        public bool IsSend { get; set; }
        /// <summary>
        /// Get created date of email
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Get created by for email
        /// </summary>
        public int CreatedBy { get; set; }
    }
}
