using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Role;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Exceptions.Staff;

namespace HospitalManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("/role")]
        public async Task<IActionResult> AddNewRole([FromBody] string roleName)
        {
            try
            {
                Role role = await _adminService.AddNewRole(roleName);
                return Ok(new SuccessResponse("Role Added"));
            } catch (InvalidRoleNameException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
            }
            catch (EntityCreationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
            catch (RoleDuplicationException ex)
            {
                return Conflict(new ErrorResponse(ex.Message, StatusCodes.Status409Conflict));
            }
        }

        [HttpPost("/staff")]
        public async Task<IActionResult> AddStaff(NewStaffDTO newStaffDTO)
        {
            try
            {
                Staff staff = await _adminService.AddStaff(newStaffDTO);
                return Ok(new SuccessResponse("Staff Added", staff));
            }
            catch (InvalidStaffInputException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
            }
            catch (EntityCreationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}
