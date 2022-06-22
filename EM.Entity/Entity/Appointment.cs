using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
        /// Getting user id   
        /// </summary>
        [DisplayName("UserId")]
        [ForeignKey ("UserId")]
       
        public int? UserId { get; set; }

        /// <summary>
        /// Getting DoctorId  
        /// </summary>
        [DisplayName("DoctorId")]
        [ForeignKey("DoctorId")]
        public int? DoctorId { get; set; }

        /// <summary>
        /// Getting Diagnosis
        /// </summary>
        [DisplayName("Diagnosis")]
        public string Diagnosis { get; set; }

        /// <summary>
        /// Getting Doctor's Remarks
        /// </summary>
        [DisplayName("Remarks")]
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
        ///  
        /// </summary>
        [DisplayName("StartDateTime")]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [DisplayName("StartDateTime")]
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

        //Foreign key purpose
        /// <summary>
        /// Get or set user table
        /// </summary>
        public virtual User User { get; set; }

        public virtual Doctor Doctor  {get; set;}
    }
}
