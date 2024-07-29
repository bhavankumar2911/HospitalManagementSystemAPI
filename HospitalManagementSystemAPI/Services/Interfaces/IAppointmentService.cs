using HospitalManagementSystemAPI.DTOs.Appointment;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Appointment> BookAppointment(NewAppointmentDTO newAppointmentDTO);
    }
}
