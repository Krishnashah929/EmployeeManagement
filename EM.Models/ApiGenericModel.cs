using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models
{
    /// <summary>
    /// API Generic Model
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public partial class ApiGenericModel<T>
    {
        /// <summary>
        /// Get the statuscode
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Get the message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets the particular model
        /// </summary>
        public T GenericModel { get; set; }
        /// <summary>
        /// Gets the list 
        /// </summary>
        public List<T> GenericList { get; set; }
        
    }
}
