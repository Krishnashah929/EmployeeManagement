namespace EM.Models
{
    /// <summary>
    /// class for JqueryDatatableParameter
    /// </summary>
    public class JqueryDatatableParam
    {
        /// <summary>
        /// for getting draw value
        /// </summary>
        public string draw { get; set; }
        /// <summary>
        /// for getting sortcoloum name
        /// </summary>
        public string sortColumn { get; set; }
        /// <summary>
        /// for getting search value 
        /// </summary>
        public string searchValue { get; set; }
        /// <summary>
        /// for getting sort coloum's direction
        /// </summary>
        public string sortColumnDirection { get; set; }
        /// <summary>
        /// for getting pagesize
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// for skip
        /// </summary>
        public int skip { get; set; }
    }
}
