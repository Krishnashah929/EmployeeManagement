#region Using
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Entity
{
    /// <summary>
    /// Field Options model
    /// </summary>
    public partial class FieldOptions
    {
        /// <summary>
        /// Get FieldOptionsId
        /// </summary>
        [Key]
        public int FieldOptionsId { get; set; }

        /// <summary>
        /// Get FieldDetailsId
        /// </summary>
        [ForeignKey("FieldDetailsId")]
        public int FieldDetailsId { get; set; }

        /// <summary>
        /// Get OptionValue
        /// </summary>
        public string OptionValue { get; set; }
    }
}