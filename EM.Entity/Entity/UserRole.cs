using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace EM.Entity
{
    /// <summary>
    /// Main UserRole entity
    /// </summary>
    public partial class UserRole
    {
        /// <summary>
        /// Get or set composite key 
        /// </summary>
        [DisplayName("Compsitekey")]
        public int Compsitekey { get; set; }

        /// <summary>
        /// Get or set user id 
        /// </summary>
        [DisplayName("User Id")]
        public int UserId { get; set; }

        /// <summary>
        /// Get or set userrole
        /// </summary>
        [DisplayName("UserRole Id")]
        public int UserRoleId { get; set; }

        /// <summary>
        /// Get or set user
        /// </summary>
        public virtual User User { get; set; }
    }
}
