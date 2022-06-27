#region Using
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace EM.Entity
{
    /// <summary>
    /// State Model
    /// </summary>
    public partial class State
    {
        /// <summary>
        /// Get StateId
        /// </summary>
        [Key]
        public int StatesId { get; set; }
        /// <summary>
        /// Get CountryId
        /// </summary>
        public int CountryId { get; set; }
        /// <summary>
        /// Get StateName
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// Reference of country table
        /// </summary>
        public Country Country { get; set; }
        /// <summary>
        /// Reference of city table
        /// </summary>
        public ICollection<City> Cities { get; set; }
    }
}
