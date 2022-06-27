namespace EM.Entity
{
    /// <summary>
    /// City Model
    /// </summary>
    public partial class City
    {
        /// <summary>
        /// Get CityId
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// Get StateId
        /// </summary>
        public int StatesId { get; set; }
        /// <summary>
        /// Get CityName
        /// </summary>
        public string CityName { get; set; }

        public State States { get; set; }
    }
}
