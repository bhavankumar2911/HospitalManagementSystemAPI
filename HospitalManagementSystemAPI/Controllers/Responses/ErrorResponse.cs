using Microsoft.AspNetCore.Mvc;

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
        public Dictionary<string, IList<string>> Errors { get; set; } = new Dictionary<string, IList<string>>();

        public static IActionResult CreateCustomErrorResponseForInvalidModel(ActionContext actionContext)
        {
            ErrorResponse errorResponse = new ErrorResponse("Invalid inputs", StatusCodes.Status400BadRequest);

            var properties = actionContext.ModelState.AsEnumerable();

            foreach ( var property in properties)
            {
                IList<string> errorMessages = new List<string>();

                foreach (var error in property.Value!.Errors)
                {
                    errorMessages.Add(error.ErrorMessage);
                }

                errorResponse.Errors.Add(property.Key, errorMessages);
            }

            return new BadRequestObjectResult(errorResponse);
        }
    }
}
