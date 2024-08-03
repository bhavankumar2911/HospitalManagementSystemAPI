using HospitalManagementSystemAPI.DTOs.Prescription;
using HospitalManagementSystemAPI.DTOs.PrescriptionItem;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;
using System.Text;

namespace HospitalManagementSystemAPI.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IRepository<Medicine> _medicineRepository;
        private readonly IRepository<Prescription> _prescriptionRepository;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<PrescriptionItem> _prescriptionItemRepository;
        private readonly IRepository<User> _userRepository;

        public PrescriptionService(IRepository<Medicine> medicineRepository, IRepository<Prescription> prescriptionRepository, IRepository<Doctor> doctorRepository, IRepository<Patient> patientRepository, IRepository<PrescriptionItem> prescriptionItemRepository, IRepository<User> userRepository)
        {
            _medicineRepository = medicineRepository;
            _prescriptionRepository = prescriptionRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _prescriptionItemRepository = prescriptionItemRepository;
            _userRepository = userRepository;
        }

        private async Task DeleteSavedPrescription (int prescriptionId)
        {
            await _prescriptionRepository.Delete(prescriptionId);
        }

        private async Task<Medicine> GetMedicineById(int medicineId, int prescriptionId)
        {
            try
            {
                // fetch medicine
                Medicine medicine = await _medicineRepository.Get(medicineId);

                return medicine;
            }
            catch (EntityNotFoundException)
            {
                await DeleteSavedPrescription(prescriptionId);
                throw;
            }
        }

        private string GetNotAddableItems (IList<string> notAddableItems)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < notAddableItems.Count - 1; i++)
            {
                sb.Append($"{notAddableItems[i]}, ");
            }

            sb.Append(notAddableItems[notAddableItems.Count - 1]);

            return sb.ToString();
        }

        private async Task<Doctor> GetDoctorFromUserId(int userId)
        {
            var staffId = (await _userRepository.Get(userId)).Staff.Id;
            var doctor = (await _doctorRepository.GetAll())
                .FirstOrDefault(d => d!.Staff.Id == staffId, null);

            if (doctor == null) throw new EntityNotFoundException("User", userId);

            return doctor;
        }

        public async Task<Prescription> SaveNewPrescription(NewPrescriptionDTO newPrescriptionDTO, int userId)
        {
            var doctor = await GetDoctorFromUserId (userId);
            var patient = await _patientRepository.Get(newPrescriptionDTO.PatientId);

            // save prescription first
            var prescription = await _prescriptionRepository.Create(new Prescription
            {
                Doctor = doctor,
                Patient = patient,
            });

            IList<string> notAddableItems = new List<string>();

            // save items
            for (int i = 0; i < newPrescriptionDTO.PrescriptionItems.Count; i++)
            {
                PrescriptionItemDTO prescriptionItem = newPrescriptionDTO.PrescriptionItems[i];

                Medicine medicine = await GetMedicineById(prescriptionItem.MedicineId, prescription.Id);

                try
                {
                    var item = await _prescriptionItemRepository.Create(new PrescriptionItem
                    {
                        Prescription = prescription,
                        DosageInMG = prescriptionItem.DosageInMG,
                        ConsumingInterval = prescriptionItem.ConsumingInterval,
                        NoOfDays = prescriptionItem.NoOfDays,
                        Medicine = medicine,
                    });
                } catch (EntityCreationException)
                {
                    notAddableItems.Add($"{medicine.Name} ({prescriptionItem.DosageInMG} mg)");
                }
            }

            if (notAddableItems.Count > 0)
            {
                await DeleteSavedPrescription(prescription.Id);
                throw new EntityCreationException(GetNotAddableItems(notAddableItems));
            }

            return prescription;
        }
    }
}
