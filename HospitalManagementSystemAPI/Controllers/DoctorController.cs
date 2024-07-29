using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("/doctor")]
        public async Task<IActionResult> ViewAllDoctors ()
        {
            try
            {
                var doctors = await _doctorService.ViewAllDoctors();
                return Ok(new SuccessResponse(doctors));
            } catch (NoEntitiesAvailableException ex)
            {
                return NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
        }

        [HttpGet("/doctors/least-appointments")]
        public async Task<IActionResult> GetDoctorsWithLeastAppointments()
        {
            try
            {
                var doctors = await _doctorService.GetDoctorsWithLeastAppointments();
                return Ok(new SuccessResponse(doctors));
            }
            catch (NoEntitiesAvailableException ex)
            {
                return NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
        }
    }
}