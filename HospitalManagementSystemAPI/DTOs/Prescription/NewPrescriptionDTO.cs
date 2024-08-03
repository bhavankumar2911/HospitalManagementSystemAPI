using HospitalManagementSystemAPI.DTOs.PrescriptionItem;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.Prescription
{
    public class NewPrescriptionDTO
    {
        [Required(ErrorMessage = "Patient is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Select a valid patient.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Prescription items is required")]
        [MinLength(1, ErrorMessage = "Atleast one prescription item is required.")]
        public IList<PrescriptionItemDTO> PrescriptionItems { get; set; } = new List<PrescriptionItemDTO>();
    }
}
