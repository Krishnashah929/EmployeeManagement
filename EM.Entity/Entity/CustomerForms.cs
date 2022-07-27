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
    [Table("CustomerForms")]
    public partial class CustomerForms
    {
        /// <summary>
        /// Get CustomerFormsId.
        /// </summary>
        [Key]
        public int CustomerFormsId { get; set; }
        
        /// <summary>
        /// Get FormId.
        /// </summary>
        [ForeignKey("FormId")]
        public int FormId { get; set; }

        /// <summary>
        /// FormName input feild.
        /// </summary>
        [DisplayName("Field Id")]
        public int FormFieldId { get; set; }

        /// <summary>
        /// CustomerEmail input feild.
        /// </summary>
        [DisplayName("Email")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// SignatureData input feild.
        /// </summary>
        [DisplayName("Signature")]
        public string SignatureData { get; set; }

        /// <summary>
        /// Isactive feild.
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// IsSubmitted feild.
        /// </summary>
        [DisplayName("Submit")]
        public bool IsSubmitted { get; set; }

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

        /// <summary>
        /// SubmitDate Date field.
        /// </summary>
        [DisplayName("Submit Date")]
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// ExpiryDate Date field.
        /// </summary>
        [DisplayName("Expiry Date")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public DateTime ExpiryDate { get; set; }

        [NotMapped]
        public string FormName { get; set; }
    }
}
