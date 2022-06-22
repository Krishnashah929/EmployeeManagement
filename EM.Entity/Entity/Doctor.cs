#region Using
using EM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Entity
{
    /// <summary>
    /// Doctor class
    /// </summary>
    [Table ("Doctors")]
    public partial class Doctor
    {
        /// <summary>
        /// For geeting doctor id 
        /// </summary>
        [DisplayName("DoctorId")]
        public int DoctorId { get; set; }

        /// <summary>
        /// For geeting User id 
        /// </summary>
        [DisplayName("UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// For geeting CityID
        /// </summary>
        [DisplayName("CityID")]
        [Required(ErrorMessage = CommonValidations.SelectRequiredErrorMsg)]
        public int CityID { get; set; }


        /// <summary>
        /// For geeting StateID
        /// </summary>
        [DisplayName("StateID")]
        [Required(ErrorMessage = CommonValidations.SelectRequiredErrorMsg)]
        public int StateID { get; set; }


        /// <summary>
        /// For geeting CountryID 
        /// </summary>
        [DisplayName("CountryID")]
        [Required(ErrorMessage = CommonValidations.SelectRequiredErrorMsg)]
        public int CountryID { get; set; }

        /// <summary>
        /// For geeting Pincode 
        /// </summary>
        [DisplayName("Pincode")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public int Pincode { get; set; }

        /// <summary>
        /// For getting Address.
        /// </summary>
        [DisplayName("Address")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public string Address { get; set; }

        /// <summary>
        /// For geeting PhoneNumber 
        /// </summary>
        [DisplayName("PhoneNumber")]
        [Required(ErrorMessage = CommonValidations.RequiredErrorMsg)]
        public decimal PhoneNumber { get; set; }

        /// <summary>
        /// For getting Color.
        /// </summary>
        [DisplayName("Color")]
        [Required(ErrorMessage = CommonValidations.SelectRequiredErrorMsg)]
        public string Color { get; set; }

        /// <summary>
        /// Created by field for first time of creation of doctor.
        /// </summary>
        [DisplayName("Created By")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Created date field for first time of doctor create account date.
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// ModifiedBy field for modiffy details of doctor.
        /// </summary>
        [DisplayName("Modified By")]
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// ModifiedDate field for doctor modify their details.
        /// </summary>
        [DisplayName("Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// FirstName input feild.
        /// </summary>
        [DisplayName("First Name")]
        [NotMapped]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName input feild.
        /// </summary>
        [DisplayName("Last Name")]
        [NotMapped]
        public string Lastname { get; set; }

        /// <summary>
        /// Email Address input feild.
        /// </summary>
        [MaxLength(50)]
        [DisplayName("Email")]
        [NotMapped]
        public string EmailAddress { get; set; }

        /// <summary>
        /// For geeting SpecialityId
        /// </summary>
        [DisplayName("SpecialityId")]
        [NotMapped]
        //[Required(ErrorMessage = CommonValidations.SelectRequiredErrorMsg)]
        public List<int> SpecialityId { get; set; }
    }
}
