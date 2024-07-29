using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Appointment;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Profiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<NewAppointmentDTO, Appointment>();
        }
    }
}
