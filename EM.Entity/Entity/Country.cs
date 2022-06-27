using System.Collections.Generic;

namespace EM.Entity
{
    /// <summary>
    /// Coutry Model
    /// </summary>
    public partial class Country
    {
        /// <summary>
        /// Get CoutryId
        /// </summary>
        public int CountryId { get; set; }
        /// <summary>
        /// Get CountryName
        /// </summary>
        public string CountryName { get; set; }
        /// <summary>
        /// Get SortName
        /// </summary>
        public string SortName { get; set; }
        /// <summary>
        /// Reference of state table
        /// </summary>
        public ICollection<State> States { get; set; }
    }
}
