#region Using
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Entity
{
    /// <summary>
    /// Get CustomerFormData Model
    /// </summary>
    public partial class CustomerFormData
    {
        /// <summary>
        /// Get CustomerFormDataId
        /// </summary>
        [Key]
        public int CustomerFormDataId { get; set; }

        /// <summary>
        /// Get CustomerFormsId
        /// </summary>
        [ForeignKey("CustomerFormsId")]
        public int CustomerFormsId { get; set; }

        /// <summary>
        /// Get FieldId
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Get FieldValue
        /// </summary>
        public string FieldValue { get; set; }
    }

    /// <summary>
    /// View Model of CustomerFormData
    /// </summary>
    public class CustomerFormDataValues
    {

        /// <summary>
        /// Get CustomerFormsId
        /// </summary>
 
        public int CustomerFormsId { get; set; }

        /// <summary>
        /// Get FieldId
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// Get FieldValue
        /// </summary>
        public string FieldValue { get; set; }
    }
}