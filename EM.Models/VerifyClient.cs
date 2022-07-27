#region Using
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Models
{
    /// <summary>
    /// Forms Entity.
    /// </summary>
    public partial class VerifyClient
    {
        /// <summary>
        /// Get FormId.
        /// </summary>
        public int FormId { get; set; }

        /// <summary>
        /// CustomerEmail input feild.
        /// </summary>
        [DisplayName("Email")]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// IsSubmitted feild.
        /// </summary>
        [DisplayName("Submit")]
        public bool IsSubmitted { get; set; }
    }
}
