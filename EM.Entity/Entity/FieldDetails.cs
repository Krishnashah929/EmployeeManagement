#region Using
using EM.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Entity 
{
    /// <summary>
    /// Form details entiy
    /// </summary>
    [Table("FieldDetails")]
    public partial class FieldDetails
    {
        /// <summary>
        /// Get field details id
        /// </summary>
        [Key]
        public int FieldDetailsId { get; set; }

        /// <summary>
        /// Get field type
        /// </summary>
        [DisplayName("Field Type")]
        [Required(ErrorMessage = CommonValidations.SelectRequiredErrorMsg)]
        public int FieldType { get; set; }

        /// <summary>
        /// Get field html name
        /// </summary>
        [DisplayName("Field Name")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string FieldHtmlName { get; set; }

        /// <summary>
        /// Get field validation type
        /// </summary>
        public int FieldValidationType { get; set; }

        /// <summary>
        /// Get form id
        /// </summary>
        [ForeignKey("FormId")]
        public int FormId { get; set; }

        /// <summary>
        /// Get is optional value
        /// </summary>
        public bool IsOptional { get; set; }

        /// <summary>
        /// Get no of. data table coloum value
        /// </summary>
        public int NoOfDatatableColumn { get; set; }

        /// <summary>
        /// Get coloum title
        /// </summary>
        public string ColumnOneTitle { get; set; }

        /// <summary>
        /// Get coloum title
        /// </summary>
        public string ColumnTwoTitle { get; set; }

        /// <summary>
        /// Get coloum title
        /// </summary>
        public string ColumnThreeTitle { get; set; }

        /// <summary>
        /// Get field image path
        /// </summary>
        public string FieldImagePath { get; set; }

        /// <summary>
        /// For getting field type  in string format for data table
        /// </summary>
        [NotMapped]
        public string fieldTypeName { get; set; }

        /// <summary>
        /// For getting field validation type  in string format for data table
        /// </summary>
        [NotMapped]
        public string fieldValidationName { get; set; }

        /// <summary>
        /// Get HelpText
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Get FormName
        /// </summary>
        public string FormName { get; set; }
    }
}