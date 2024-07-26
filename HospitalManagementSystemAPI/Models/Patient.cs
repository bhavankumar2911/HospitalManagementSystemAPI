using HospitalManagementSystemAPI.Enums;

namespace HospitalManagementSystemAPI.Models
{
    public class Patient
    {
        public Patient() { }

        public int Id { get; set; }
        public Gender Gender { get; set; }
        public Blood Blood { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
