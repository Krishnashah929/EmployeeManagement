#region Using
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
#endregion

namespace EM.Entity
{
    /// <summary>
    /// Speciality Model
    /// </summary>
    [Table ("Specialities")]
    public partial class Speciality
    {
        /// <summary>
        /// For geeting doctor id 
        /// </summary>
        [DisplayName("DoctorId")]
        public int DoctorId { get; set; }

        /// <summary>
        /// For geeting SpecialityId
        /// </summary>
        [DisplayName("SpecialityId")]
        public int SpecialityId { get; set; }
    }
}
