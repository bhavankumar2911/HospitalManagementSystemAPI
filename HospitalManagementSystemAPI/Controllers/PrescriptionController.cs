using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.DTOs.Prescription;
using HospitalManagementSystemAPI.Exceptions.Authentication;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpPost("/prescription")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> SaveNewPrescription(NewPrescriptionDTO newPrescriptionDTO)
        {
            var id = HttpContext.User.Claims.First(c => c.Type == "id").Value;

            try
            {
                var prescription = await _prescriptionService.SaveNewPrescription(newPrescriptionDTO, int.Parse(id));

                return Ok(new SuccessResponse("Prescription saved", prescription));
            } catch (Exception ex)
            {
                return ex switch
                {
                    EntityNotFoundException => NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound)),

                    EntityCreationException => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message, StatusCodes.Status500InternalServerError)),

                    _ => StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse("Unknown error occurred.", StatusCodes.Status500InternalServerError))
                };
            }
        }

        [HttpPost("/patient/prescription")]
        public async Task<IActionResult> GetPatientPrescriptions(PatientPrescriptionsInputDTO patientPrescriptionsInputDTO)
        {
            try
            {
                var prescriptions = await _prescriptionService.GetPatientPrescriptions(patientPrescriptionsInputDTO);

                return Ok(new SuccessResponse(prescriptions));
            } catch (NoEntitiesAvailableException ex)
            {
                return NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            } catch (InvalidLoginCredentialsException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpGet("/doctor/patient/prescription/{patientId}")]
        //[Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorPatientPrescriptions (int patientId)
        {
            try
            {
                var id = HttpContext.User.Claims.First(c => c.Type == "id").Value;

                var prescriptions = await _prescriptionService.GetDoctorPatientPrescriptions(int.Parse(id), patientId);

                return Ok(new SuccessResponse(prescriptions));
            }
            catch (Exception ex)
            {
                return NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
        }
    }
}
