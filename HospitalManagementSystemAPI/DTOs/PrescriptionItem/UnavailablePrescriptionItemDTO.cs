using HospitalManagementSystemAPI.Enums;

namespace HospitalManagementSystemAPI.DTOs.PrescriptionItem
{
    public class UnavailablePrescriptionItemDTO
    {
        public string MedicineName { get; set; } = string.Empty;
        public int DosageInMG { get; set; }
        public ConsumingInterval ConsumingInterval { get; set; }
        public int NoOfDays { get; set; }
    }
}
