using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;

namespace HospitalManagementSystemAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorService(IRepository<Doctor> doctorRepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task AddNewDoctor(Doctor doctor)
        {
            await _doctorRepository.Create(doctor);
        }

        public async Task<IEnumerable<DoctorResponseDTO>> ViewAllDoctors()
        {
            var doctors = (await _doctorRepository.GetAll())
                .Select(doctor =>
                {
                    DoctorResponseDTO doctorResponseDTO = _mapper.Map<DoctorResponseDTO>(doctor);
                    doctorResponseDTO = _mapper.Map<Staff, DoctorResponseDTO>(doctor.Staff, doctorResponseDTO);

                    return doctorResponseDTO;
                });

            //var doctors = await _doctorRepository.GetAll();

            return doctors;
        }
    }
}
