using HospitalManagementSystemAPI.DTOs.MedicalHistory;
using HospitalManagementSystemAPI.DTOs.Patient;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Patient;
using HospitalManagementSystemAPI.Exceptions.Staff;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;

namespace HospitalManagementSystemAPI.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<MedicalHistory> _medicalHistoryRepository;

        public PatientService(IRepository<Patient> patientRepository, IRepository<MedicalHistory> medicalHistoryRepository)
        {
            _patientRepository = patientRepository;
            _medicalHistoryRepository = medicalHistoryRepository;
        }

        private async Task CheckEmailPhoneDuplication(string email, string phone)
        {
            try
            {
                var patients = await _patientRepository.GetAll();

                // email check
                if (patients.Any(patient => patient.Email == email)) throw new PatientEmailDuplicationException();

                // phone check
                if (patients.Any(patient => patient.Phone == phone)) throw new PatientPhoneDuplicationException();
            }
            catch (NoEntitiesAvailableException) { }
        }

        private Patient MapPatient(NewPatientDTO newPatientDTO)
        {
            return new Patient
            {
                Firstname = newPatientDTO.Firstname,
                Lastname = newPatientDTO.Lastname,
                Email = newPatientDTO.Email,
                Phone = newPatientDTO.Phone,
                Blood = newPatientDTO.Blood,
                Gender = newPatientDTO.Gender,
            };
        }

        private bool HasMedicalHistory (PatientMedicalHistoryDTO medicalHistory)
        {
            if ((medicalHistory.PreConditions == null || medicalHistory.PreConditions == string.Empty)
                && (medicalHistory.Allergies == null || medicalHistory.Allergies == string.Empty))
                return false;

            return true;
        }

        public async Task<Patient> AddNewPatient(NewPatientDTO newPatientDTO)
        {
            await CheckEmailPhoneDuplication(newPatientDTO.Email, newPatientDTO.Phone);

            // save patient
            Patient patient = await _patientRepository.Create(MapPatient(newPatientDTO));

            // save medical history
            if (HasMedicalHistory(newPatientDTO.MedicalHistory))
            {
                MedicalHistory medicalHistory = new MedicalHistory(
                        newPatientDTO.MedicalHistory.PreConditions,
                        newPatientDTO.MedicalHistory.Allergies,
                        patient
                    );

                await _medicalHistoryRepository.Create(medicalHistory);
            }

            return patient;
        }

        public Task<Patient> SearchPatientByName(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
