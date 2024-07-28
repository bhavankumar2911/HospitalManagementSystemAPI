namespace HospitalManagementSystemAPI.Models
{
    public class Nurse
    {
        public int Id { get; set; }
        public Patient? Patient { get; set; }
        public Staff Staff { get; set; } = new Staff();
    }
}
