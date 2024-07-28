using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Profiles
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<Doctor, DoctorResponseDTO>();
            CreateMap<Staff, DoctorResponseDTO>();
        }
    }
}
