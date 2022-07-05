#region Using
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Models
{
    /// <summary>
    /// Check dtae time when user drag and drop any event
    /// </summary>
    public partial class DragAndDrop
    {
        /// <summary>
        /// For geeting doctor id 
        /// </summary>
        [DisplayName("AppointmentId")]
        public int AppointmentId { get; set; }
        /// <summary>
        /// Getting DoctorId  
        /// </summary>
        [DisplayName("Doctor Name")]
        [ForeignKey("DoctorId")]
        public int? DoctorId { get; set; }

        /// <summary>
        ///  Get StartDateTime
        /// </summary>
        [DisplayName("StartDateTime")]
        public DateTime StartDateTime { get; set; }

        /// <summary>
        ///  Get EndDateTime
        /// </summary>
        [DisplayName("EndDateTime")]
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Current Date
        /// </summary>
        [DisplayName("CurrentDate")]
        public DateTime CurrentDate { get; set; }
    }
}