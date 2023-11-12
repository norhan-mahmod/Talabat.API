namespace Talabat.API.Errors
{
    public class ApiValidationErrorResponse :ApiErrorResponse
    {
        public List<string> Errors { get; set; }
        public ApiValidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }

    }
}
