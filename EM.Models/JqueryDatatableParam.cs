using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Models
{
    public class JqueryDatatableParam
    {
        public string draw { get; set; }
        public string sortColumn { get; set; }
        public string searchValue { get; set; }
        public string sortColumnDirection { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }
    }
}
