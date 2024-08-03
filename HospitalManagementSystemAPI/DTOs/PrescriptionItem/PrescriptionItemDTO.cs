using HospitalManagementSystemAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.PrescriptionItem
{
    public class PrescriptionItemDTO
    {
        [Required(ErrorMessage = "Medicine is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Select a valid medicine.")]
        public int MedicineId { get; set; }

        [Required(ErrorMessage = "Medicine dosage is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Give a valid dosage in milligram.")]
        public int DosageInMG { get; set; }

        [Required(ErrorMessage = "Consuming Interval is required")]
        [Range(0, 3, ErrorMessage = "Give a valid consuming interval")]
        public ConsumingInterval ConsumingInterval { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Give a valid number of days")]
        public int NoOfDays { get; set; }
    }
}
