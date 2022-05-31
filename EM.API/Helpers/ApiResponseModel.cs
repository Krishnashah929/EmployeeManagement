namespace EM.API.Helpers
{
    public class ApiResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string DataObj { get; set; }
        public string DataList { get; set; }
    }
}