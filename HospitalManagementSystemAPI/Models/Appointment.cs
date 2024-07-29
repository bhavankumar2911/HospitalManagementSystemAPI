using HospitalManagementSystemAPI.Enums;

namespace HospitalManagementSystemAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime BookedDateTime { get; set; } = DateTime.Now;
        public DateTime FixedDateTime { get; set; }
        public Doctor Doctor { get; set; } = new Doctor();
        public Patient Patient { get; set; } = new Patient();
        public string Concern { get; set; } = string.Empty;
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Fixed;
    }
}
