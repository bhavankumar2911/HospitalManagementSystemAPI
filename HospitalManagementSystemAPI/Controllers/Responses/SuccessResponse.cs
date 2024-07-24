namespace HospitalManagementSystemAPI.Controllers.Responses
{
    public class SuccessResponse
    {
        public SuccessResponse(string message, object data)
        {
            Message = message;
            Data = data;
        }

        public SuccessResponse(string message)
        {
            Message = message;
        }

        public SuccessResponse(object data)
        {
            Data = data;
        }

        public int StatusCode { get; } = 200;
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
