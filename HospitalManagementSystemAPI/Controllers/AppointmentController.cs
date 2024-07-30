using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.DTOs.Appointment;
using HospitalManagementSystemAPI.Exceptions.Doctor;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Patient;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("/appointment")]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BookAppointment (NewAppointmentDTO newAppointmentDTO)
        {
            try
            {
                Appointment appointment = await _appointmentService.BookAppointment(newAppointmentDTO);

                return Ok(new SuccessResponse("Appointment fixed.", appointment));
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    EntityCreationException => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message, StatusCodes.Status500InternalServerError)),

                    EntityNotFoundException or DoctorAppointmentOverflowException => NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound)),

                    DoctorNotAvailableException or PatientAppointmentConflictException => Conflict(new ErrorResponse(ex.Message, StatusCodes.Status409Conflict)),

                    _ => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("Unknown error occurred.", StatusCodes.Status500InternalServerError)),
                }; ;
            }
        }
    }
}
