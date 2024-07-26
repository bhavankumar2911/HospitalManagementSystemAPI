using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.DTOs.Patient;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Patient;
using HospitalManagementSystemAPI.Exceptions.Staff;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("/patient")]
        public async Task<IActionResult> AddNewPatient(NewPatientDTO newPatientDTO)
        {
            try
            {
                Patient patient = await _patientService.AddNewPatient(newPatientDTO);
                return Ok(new SuccessResponse("Patient Added", patient));
            }
            catch (EntityCreationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message, StatusCodes.Status500InternalServerError));
            }
            catch (PatientEmailDuplicationException ex)
            {
                return Conflict(new ErrorResponse(ex.Message, StatusCodes.Status409Conflict));
            }
            catch (PatientPhoneDuplicationException ex)
            {
                return Conflict(new ErrorResponse(ex.Message, StatusCodes.Status409Conflict));
            }
        }
    }
}
