using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Appointment;
using HospitalManagementSystemAPI.Enums;
using HospitalManagementSystemAPI.Exceptions.Doctor;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Patient;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;

namespace HospitalManagementSystemAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IRepository<Appointment> appointmentRepository, IRepository<Doctor> doctorRepository, IRepository<Patient> patientRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        //private async Task CheckDoctorAvailability (DateTime appointmentFixingTime)
        //{
        //    var futureAppointments = (await _appointmentRepository.GetAll())
        //        .Where(appointment => appointment.FixedDateTime > DateTime.Now);
        //    var doctors = await _doctorRepository.GetAll();
        //    var doctorsWithoutAppointments = doctors
        //        .Join(futureAppointments, doctor => doctor.Id,
        //        futureAppointment => futureAppointment.Doctor.Id,
        //        (futureAppointment, doctor) => new { Doctor = doctor, Appointment = futureAppointment });

        //    foreach (var item in doctorsWithoutAppointments)
        //    {
        //        await Console.Out.WriteLineAsync((char)item.Doctor.Id);
        //        await Console.Out.WriteLineAsync((char)item.Appointment.Id);
        //    }
        //}

        private async Task CheckAppointmentConflicts(DateTime appointmentFixingTime, int doctorId)
        {
            try
            {
                var appointments = (await _appointmentRepository.GetAll())
                    .Where(a => (a.Doctor.Id == doctorId) && a.AppointmentStatus == AppointmentStatus.Fixed);

                foreach (var appointment in appointments)
                {
                    DateTime appointmentEndTime = appointment.FixedDateTime.AddMinutes(30);

                    if (appointmentEndTime > appointmentFixingTime) throw new DoctorNotAvailableException();
                }
            } catch (NoEntitiesAvailableException) { }
        }

        private async Task CheckPatientConflicts (int patientId)
        {
            try
            {
                var appointments = (await _appointmentRepository.GetAll())
                    .Where(a => a.Patient.Id == patientId
                    && (a.AppointmentStatus == AppointmentStatus.Fixed
                    || a.AppointmentStatus == AppointmentStatus.Admitted));

                if (appointments.Any())
                {
                    if (appointments.First().AppointmentStatus == AppointmentStatus.Fixed)
                        throw new PatientAppointmentConflictException("The patient already has an appointment.");

                    throw new PatientAppointmentConflictException("The patient is already admitted");
                }
                       
            } catch (NoEntitiesAvailableException) { }
        }

        public async Task<Appointment> BookAppointment(NewAppointmentDTO newAppointmentDTO)
        {
            Patient patient = await _patientRepository.Get(newAppointmentDTO.PatientId);
            Doctor doctor = await _doctorRepository.Get(newAppointmentDTO.DoctorId);

            // patient conflicts
            await CheckPatientConflicts(newAppointmentDTO.PatientId);

            // check conflicts
            await CheckAppointmentConflicts(newAppointmentDTO.FixedDateTime, newAppointmentDTO.DoctorId);

            Appointment appointment = _mapper.Map<Appointment>(newAppointmentDTO);
            appointment.Patient = patient;
            appointment.Doctor = doctor;

            appointment = await _appointmentRepository.Create(appointment);

            return appointment;
        }
    }
}
