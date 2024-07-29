namespace HospitalManagementSystemAPI.DTOs.Doctor
{
    public class DoctorResponseDTO
    {
        public int Id { get; set; }
        public string Qualification { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
    }
}
