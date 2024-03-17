using System.Net;

namespace Core
{
    public class ResponseResult
    {
        public bool Success { get; set; }
        public bool IsSuccess { get { return Success; } }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
        //public long RecordsEffected { get; set; }
        //public long TotalRecords { get; set; }
        public ResponseResult()
        {
            Success = false;
            Message = "";
            StatusCode = 0;
            Data = null;
            //RecordsEffected = 0;
            //TotalRecords = 0;

        }
    }
}
