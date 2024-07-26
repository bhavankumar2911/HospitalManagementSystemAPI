namespace HospitalManagementSystemAPI.Models
{
    public class MedicalHistory
    {
        public MedicalHistory()
        {
        }

        public MedicalHistory(string preConditions, string allergies, Patient patient)
        {
            PreConditions = preConditions;
            Allergies = allergies;
            Patient = patient;
        }

        public int Id { get; set; }
        public string PreConditions { get; set; } = string.Empty;
        public string Allergies { get; set; } = string.Empty;
        public Patient Patient { get; set; } = new Patient();
    }
}
