using HospitalManagementSystemAPI.DTOs.PrescriptionItem;

namespace HospitalManagementSystemAPI.DTOs.Prescription
{
    public class UnavailableMedicinePrescriptionDTO
    {
        public int PatientId { get; set; }
        public IList<UnavailablePrescriptionItemDTO> PrescriptionItems { get; set; } = new List<UnavailablePrescriptionItemDTO>();
    }
}
