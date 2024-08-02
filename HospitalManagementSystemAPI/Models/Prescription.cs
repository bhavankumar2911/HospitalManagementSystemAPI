namespace HospitalManagementSystemAPI.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public Doctor Doctor { get; set; } = new Doctor();
        public Patient Patient { get; set; } = new Patient();
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
