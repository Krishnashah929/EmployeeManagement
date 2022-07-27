#region Using
using EM.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Entity
{
    /// <summary>
    /// Forms Entity.
    /// </summary>
    [Table("Forms")]
    public partial class Forms
    {
        /// <summary>
        /// Get FormId.
        /// </summary>
        [Key]
        public int FormId { get; set; }

        /// <summary>
        /// FormName input feild.
        /// </summary>
        [DisplayName("Form Name")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string FormName { get; set; }

        /// <summary>
        /// DestinationEmail input feild.
        /// </summary>
        [DisplayName("Email")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string DestinationEmail { get; set; }

        /// <summary>
        /// Isactive feild.
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Created by field.
        /// </summary>
        [DisplayName("Created By")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Created date field.
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Updated By field.
        /// </summary>
        [DisplayName("Updated By")]
        public int UpdatedBy { get; set; }

        /// <summary>
        /// Updated Date field.
        /// </summary>
        [DisplayName("Updated Date")]
        public DateTime UpdatedDate { get; set; }
    }
}
