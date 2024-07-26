using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("/role")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _roleService.GetRoles();
                return Ok(new SuccessResponse(roles));
            }
            catch (NoEntitiesAvailableException ex)
            {
                return NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
        }
    }
}
