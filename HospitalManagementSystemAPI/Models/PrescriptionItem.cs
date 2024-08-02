namespace HospitalManagementSystemAPI.Models
{
    public class PrescriptionItem
    {
        public int Id { get; set; }
        public Prescription Prescription { get; set; } = new Prescription();
        public Medicine Medicine { get; set; } = new Medicine();
        public int DosageInMG { get; set; }
        public int IntervalInHours { get; set; }
    }
}
