﻿using HospitalManagementSystemAPI.DTOs.Appointment;
using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IAppointmentService
    {
        public Task<Appointment> BookAppointment(NewAppointmentDTO newAppointmentDTO);

        public Task<IEnumerable<DoctorResponseDTO>> GetAvailableDoctors(DateTime appointmentFixingDateTime);

        public Task<IEnumerable<Appointment>> GetUpcomingAppointments();

        public Task<IEnumerable<Appointment>> GetAppointmentsOfADoctor(int userId);

        public Task CloseAppointmentByDoctor(int userId, int appointmentId);
    }
}
