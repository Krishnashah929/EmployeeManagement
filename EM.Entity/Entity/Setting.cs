#region using
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Entity
{
    /// <summary>
    /// Defines the <see cref="Setting" />.
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Gets or sets the SettingId.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }

        /// <summary>
        /// Gets or sets the SettingType.
        /// </summary>
        [Display(Name = "Setting Type")]
        public short SettingType { get; set; }

        /// <summary>
        /// Gets or sets the SettingName.
        /// </summary>
        [NotMapped]
        [Display(Name = "Setting Name")]
        public string SettingName { get; set; }

        /// <summary>
        /// Gets or sets the SettingValue.
        /// </summary>
        [Display(Name = "Setting Value")]
        public string SettingValue { get; set; }

        /// <summary>
        /// Gets or sets the SettingCode.
        /// </summary>
        [Display(Name = "Setting Code")]
        public string SettingCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsActive.
        /// </summary>
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy.
        /// </summary>
        public long CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate.
        /// </summary>
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy.
        /// </summary>        
        public Nullable<long> ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate.
        /// </summary>
        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }
}
