using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Exceptions.Staff;
using HospitalManagementSystemAPI.DTOs.Doctor;

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

        [HttpPost("/staff")]
        public async Task<IActionResult> AddStaff(NewStaffDTO newStaffDTO)
        {
            try
            {
                Staff staff = await _adminService.AddStaff(newStaffDTO);
                return Ok(new SuccessResponse("Staff Added", staff));
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return ex switch
                {
                    InvalidStaffInputException => BadRequest(new ErrorResponse(ex.Message, StatusCodes.Status400BadRequest)),

                    StaffEmailDuplicationException or StaffPhoneDuplicationException => Conflict(new ErrorResponse(ex.Message, StatusCodes.Status409Conflict)),

                    EntityCreationException => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message, StatusCodes.Status500InternalServerError)),

                    _ => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("Unknown error occurred.", StatusCodes.Status500InternalServerError)),
                };
            }
        }

        [HttpPost("/doctor")]
        public async Task<IActionResult> AddStaff(NewDoctorDTO newDoctorDTO)
        {
            try
            {
                Staff staff = await _adminService.AddStaff(newDoctorDTO);
                return Ok(new SuccessResponse("Doctor Added", staff));
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    InvalidStaffInputException => BadRequest(new ErrorResponse(ex.Message, StatusCodes.Status400BadRequest)),

                    StaffEmailDuplicationException or StaffPhoneDuplicationException => Conflict(new ErrorResponse(ex.Message, StatusCodes.Status409Conflict)),

                    EntityCreationException => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message, StatusCodes.Status500InternalServerError)),

                    _ => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("Unknown error occurred.", StatusCodes.Status500InternalServerError)),
                };
            }
        }
    }
}
