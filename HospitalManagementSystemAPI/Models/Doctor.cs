namespace HospitalManagementSystemAPI.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Qualification { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public Staff Staff { get; set; } = new Staff();
    }
}
