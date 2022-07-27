namespace EM.Models
{
    /// <summary>
    /// View model for search forms
    /// </summary>
    public partial class SearchForms
    {
        /// <summary>
        /// FormName input feild.
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// DestinationEmail input feild.
        /// </summary>
         
        public string DestinationEmail { get; set; }

        /// <summary>
        /// Isactive feild.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Created by field.
        /// </summary>
        public int CreatedBy { get; set; }
    }
}
