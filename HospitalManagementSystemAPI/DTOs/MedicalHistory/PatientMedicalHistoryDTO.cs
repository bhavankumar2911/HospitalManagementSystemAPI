using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.MedicalHistory
{
    public class PatientMedicalHistoryDTO
    {
        public string PreConditions { get; set; } = string.Empty;

        public string Allergies { get; set; } = string.Empty;
    }
}
