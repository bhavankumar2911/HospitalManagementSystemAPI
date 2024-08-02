using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.DTOs.Appointment;
using HospitalManagementSystemAPI.Exceptions.Doctor;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Patient;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
                };
            }
        }

        [HttpGet("/appointment/upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var appointments = await _appointmentService.GetUpcomingAppointments();

            return Ok(new SuccessResponse(appointments));
        }

        [HttpGet("/doctor/appointments")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAppointmentsOfADoctor()
        {
            var id = HttpContext.User.Claims.First(c => c.Type == "id");

            try
            {
                var appointments = await _appointmentService.GetAppointmentsOfADoctor(int.Parse(id.Value));

                return Ok(new SuccessResponse(appointments));
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    NoEntitiesAvailableException or EntityNotFoundException => NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound)),

                    _ => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("Unknown error occurred.", StatusCodes.Status500InternalServerError))
                };
            }
        }
    }
}
