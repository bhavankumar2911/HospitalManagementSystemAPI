using HospitalManagementSystemAPI.Controllers.Responses;
using HospitalManagementSystemAPI.DTOs.Medicine;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Medicine;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [HttpPost("/medicine")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewMedicine(NewMedicineDTO newMedicineDTO)
        {
            try
            {
                var medicine = await _medicineService.AddNewMedicine(newMedicineDTO);

                return Ok(new SuccessResponse("Saved medicine", medicine));
            } catch (MedicineDuplicationException ex)
            {
                return Conflict(new ErrorResponse(ex.Message, StatusCodes.Status409Conflict));
            }
        }

        [HttpGet("/medicine")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAllMedicines()
        {
            try
            {
                var medicines = await _medicineService.GetAllMedicines();

                return Ok(new SuccessResponse(medicines));
            }
            catch(NoEntitiesAvailableException ex)
            {
                return NotFound(new ErrorResponse(ex.Message, StatusCodes.Status404NotFound));
            }
        }
    }
}
