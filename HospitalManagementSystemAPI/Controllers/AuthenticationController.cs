using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.DTOs.Authentication;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Exceptions.Authentication;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> LoginStaff(StaffLoginInputDTO staffLoginInputDTO)
        {
            try
            {
                LoginResponseDTO loginResponseDTO = await _authenticationService.LoginStaff(staffLoginInputDTO);

                return Ok(new SuccessResponse (loginResponseDTO));
            } catch (InvalidLoginCredentialsException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpGet("/authenticate")]
        [Authorize]
        public IActionResult Authorize()
        {
            var role = HttpContext.User.FindFirst(ClaimTypes.Role);
            var id = HttpContext.User.Claims.First(c => c.Type == "id");

            AuthorizeResponseDTO authorizeResponseDTO = new AuthorizeResponseDTO
            {
                Id = id.Value,
                Role = role!.Value
            };

            return Ok(new SuccessResponse(authorizeResponseDTO));
        }
    }
}
