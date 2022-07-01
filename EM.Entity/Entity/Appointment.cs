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
    /// For appointment details
    /// </summary>
    public partial class Appointment
    {
        /// <summary>
        /// For geeting doctor id 
        /// </summary>
        [DisplayName("AppointmentId")]
        public int AppointmentId { get; set; }

        /// <summary>
        /// FirstName input feild.
        /// </summary>
        [DisplayName("First Name")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName input feild.
        /// </summary>
        [DisplayName("Last Name")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string LastName { get; set; }

        /// <summary>
        /// Email Address input feild.
        /// </summary>
        [DisplayName("Email")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// For geeting PhoneNumber 
        /// </summary>
        [DisplayName("Phone Number")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public decimal PhoneNumber { get; set; }

        /// <summary>
        /// Getting DoctorId  
        /// </summary>
        [DisplayName("Doctor Name")]
        [ForeignKey("DoctorId")]
        [Required(ErrorMessage = CommonValidations.SelectRequiredErrorMsg)]
        public int? DoctorId { get; set; }

        /// <summary>
        /// Getting Diagnosis
        /// </summary>
        [DisplayName("Diagnosis")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string Diagnosis { get; set; }

        /// <summary>
        /// Getting Doctor's Remarks
        /// </summary>
        [DisplayName("Remarks")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string Remarks { get; set; }

        /// <summary>
        /// Created by field for first time of creation of user.
        /// </summary>
        [DisplayName("Created By")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Created date field for first time of user create account date.
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///  Get StartDateTime
        /// </summary>
        [DisplayName("StartDateTime")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        ///  Get EndDateTime
        /// </summary>
        [DisplayName("EndDateTime")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// ModifiedBy field for modiffy details of user.
        /// </summary>
        [DisplayName("Modified By")]
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// ModifiedDate field for user modify their details.
        /// </summary>
        [DisplayName("Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// For getting Color.
        /// </summary>
        [DisplayName("Color")]
        [NotMapped]
        public string Color { get; set; }

        //Foreign key purpose
        /// <summary>
        /// Get or set Doctor table
        /// </summary>
        public virtual Doctor Doctor  {get; set;}
    }
}