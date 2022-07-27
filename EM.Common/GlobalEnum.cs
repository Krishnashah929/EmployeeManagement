namespace EM.Common
{
    /// <summary>
    /// Class for GlobalEnum
    /// </summary>
    public class GlobalEnum
    {
        /// <summary>
        /// For global user roles
        /// </summary>
        public enum UserRoles
        {
            Admin = 1, Users = 2, Receptionist = 3, Doctors = 4
        }
        /// <summary>
        /// For global doctor colors
        /// </summary>
        public enum doctorColors
        {
            Red = 1, Blue = 2, Green = 3
        }
       
        /// <summary>
        /// For global field type in form
        /// </summary>
        public enum fieldTypes
        {
            Textbox = 1, Textarea = 2, Dropdown = 3, Radiobutton = 4, Checkbox = 5, Datatable = 6
        }

        /// <summary>
        /// For global field validation type in form
        /// </summary>
        public enum fieldValidationTypes
        {
           NA = 0, Numeric = 1, Email = 2 
        }
    }
}