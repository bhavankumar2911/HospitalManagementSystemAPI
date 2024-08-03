using HospitalManagementSystemAPI.Enums;

namespace HospitalManagementSystemAPI.Models
{
    public class UnavailablePrescriptionItem
    {
        public int Id { get; set; }
        public Prescription Prescription { get; set; } = new Prescription();
        public string MedicineName { get; set; } = string.Empty;
        public int DosageInMG { get; set; }
        public ConsumingInterval ConsumingInterval { get; set; }
        public int NoOfDays { get; set; }
    }
}
