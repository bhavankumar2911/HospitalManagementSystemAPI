using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Authentication;
using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI
{
    public class StaffProfile : Profile
    {
        public StaffProfile()
        {
            CreateMap<NewDoctorDTO, NewStaffDTO>();
            CreateMap<NewStaffDTO, Staff>();
            CreateMap<NewDoctorDTO, Staff>();
            CreateMap<Staff, LoginResponseDTO>();
        }
    }
}
