using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.DTOs.Prescription;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    }
}
