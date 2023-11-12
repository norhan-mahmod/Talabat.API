namespace Talabat.API.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiErrorResponse(int statusCode , string? message = null)
        {
            StatusCode = statusCode ;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        public string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad Request, You have made",
                401 => "Authorized , you are not",
                404 => "Resources not found",
                500 => "There is Server error",
                _ => null  //default
            };
        }
    }
}
