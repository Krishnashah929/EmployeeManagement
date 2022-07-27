#region Using
using EM.Common;
using EM.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Models
{
    /// <summary>
    /// Client side form details view model
    /// </summary>
    public partial class ClientSideFormDetails
    {
        /// <summary>
        /// Get field details id
        /// </summary>
        public int FieldDetailsId { get; set; }

        /// <summary>
        /// Get field type
        /// </summary>
        public int FieldType { get; set; }

        /// <summary>
        /// Get field html name
        /// </summary>
        [Required(ErrorMessage = CommonValidations.SelectRequiredErrorMsg)]
        public string FieldHtmlName { get; set; }

        /// <summary>
        /// Get field validation type
        /// </summary>
        public int FieldValidationType { get; set; }

        /// <summary>
        /// Get form id
        /// </summary>
        public int FormId { get; set; }

        /// <summary>
        /// Get is optional value
        /// </summary>
        public bool IsOptional { get; set; }
 
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
       
        //////////////////Form field coloums///////////////////////////
        
        /// <summary>
        /// Get FormFieldId
        /// </summary>
        public int FormFieldId { get; set; }

        /// <summary>
        /// Get FieldsSequence
        /// </summary>
        public string FieldsSequence { get; set; }

        /// <summary>
        /// Get IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Get FieldValue
        /// </summary>
        public string FieldValue { get; set; }

        /// <summary>
        /// Get HelpText
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Get list of fieldOptions
        /// </summary>
        public  List<FieldOptions> FieldOptions { get; set; }

        /// <summary>
        /// Get FormName
        /// </summary>
        public string FormName { get; set; }
    }
}