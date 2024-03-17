using System.Net;

namespace Core
{
    public static class ResponseFactory
    {
        public static ResponseResult CreateSuccess(string message, object data = null)
        {
            return new ResponseResult
            {
                Success = true,
                Message = message,
                StatusCode = HttpStatusCode.OK,
                Data = data
            };
        }

        public static ResponseResult CreateError(string message, HttpStatusCode statusCode, object data = null)
        {
            return new ResponseResult
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Data = data
            };
        }
        public static ResponseResult CreateError(Exception ex, HttpStatusCode statusCode)
        {
            return new ResponseResult
            {
                Success = false,
                Message = ex.Message,
                StatusCode = statusCode,
                Data = ex
            };
        }
        // You can add more specific methods as needed, for example:
        public static ResponseResult CreateNotFound(string message)
        {
            return CreateError(message, HttpStatusCode.NotFound);
        }
        public static ResponseResult CreateForbidden(string message)
        {
            return CreateError(message, HttpStatusCode.Forbidden);
        }
        public static ResponseResult CreateBadRequest(string message)
        {
            return CreateError(message, HttpStatusCode.BadRequest);
        }


        public static ResponseResult CreateInternalServerError(string message)
        {
            return CreateError(message, HttpStatusCode.InternalServerError);
        }
    }

}
