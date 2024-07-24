namespace HospitalManagementSystemAPI.Controllers.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
