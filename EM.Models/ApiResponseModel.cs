namespace EM.Models
{
    /// <summary>
    /// ApiResponse model 
    /// </summary>
    public class ApiResponseModel
    {
        /// <summary>
        /// get the statuscode
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// get the message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// gett the data object
        /// </summary>
        public string DataObj { get; set; }
        /// <summary>
        /// get data list 
        /// </summary>
        public string DataList { get; set; }
        /// <summary>
        /// get total record in datatable
        /// </summary>
        public int RecordsTotal { get; set; }
        /// <summary>
        /// get value of draw in datatable
        /// </summary>
        public string Draw { get; set; }
        /// <summary>
        /// get total record after searcvh in datatable
        /// </summary>
        public int RecordsFiltered { get; set; }
        /// <summary>
        /// for storing value of data 
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// For jwt token
        /// </summary>
        public string Token { get; set; }

        public ApiResponseModel()
        {

        }

        /// <summary>
        /// For jqueryDatatable's value storing 
        /// </summary>
        /// <param name="draw"></param>
        /// <param name="recordsTotal"></param>
        /// <param name="recordsFiltered"></param>
        /// <param name="dataList"></param>
        public ApiResponseModel(string draw, int recordsTotal, int recordsFiltered, string dataList)
        {
            Draw = draw;
            RecordsTotal = recordsTotal;
            RecordsFiltered = recordsFiltered;
            DataList = dataList;
        }
    }
}