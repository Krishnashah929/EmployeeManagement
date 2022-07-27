#region Using
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Entity
{
    [Table("FormFields")]
    public partial class FormFields
    {
        /// <summary>
        /// Get FormFieldId
        /// </summary>
        [Key]
        public int FormFieldId { get; set; }

        /// <summary>
        /// Get FieldsSequence
        /// </summary>
        public string FieldsSequence { get; set; }

        /// <summary>
        /// Get FormId
        /// </summary>
        [ForeignKey("FormId")]
        public int FormId { get; set; }

        /// <summary>
        /// Get IsActive
        /// </summary>
        public bool IsActive { get; set; }
    }
}
