namespace HospitalManagementSystemAPI.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
    }
}
