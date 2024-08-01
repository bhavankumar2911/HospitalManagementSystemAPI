using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Appointment;
using HospitalManagementSystemAPI.DTOs.Doctor;
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

        private void CheckAppointmentConflicts(IEnumerable<Appointment> appointments, DateTime appointmentFixingTime, int doctorId)
        {
            appointments = appointments
                    .Where(a => (a.Doctor.Id == doctorId) && a.AppointmentStatus == AppointmentStatus.Fixed);

            foreach (var appointment in appointments)
            {
                DateTime appointmentEndTime = appointment.FixedDateTime.AddMinutes(30);

                if (appointmentEndTime > appointmentFixingTime) throw new DoctorNotAvailableException();
            }
        }

        private void CheckPatientConflicts (IEnumerable<Appointment> appointments, int patientId)
        {
            appointments = appointments
                    .Where(a => a.Patient.Id == patientId
                    && (a.AppointmentStatus == AppointmentStatus.Fixed
                    || a.AppointmentStatus == AppointmentStatus.Admitted));

            if (appointments.Any())
            {
                if (appointments.First().AppointmentStatus == AppointmentStatus.Fixed)
                    throw new PatientAppointmentConflictException("The patient already has an appointment.");

                throw new PatientAppointmentConflictException("The patient is already admitted");
            }
        }

        private DateTime GetAppointmentFixingTime (IEnumerable<Appointment> appointments, int doctorId)
        {
            appointments = appointments
                    .Where(a => (a.Doctor.Id == doctorId)
                    && a.AppointmentStatus == AppointmentStatus.Fixed)
                    .OrderBy(a => a.FixedDateTime);

            // fix appointment in the next 15 minutes.
            DateTime appointmentFixingTime = DateTime.Now.AddMinutes(15);

            // 8:30 pm - 9:00 last appointment
            DateTime appointmentCloseTime = DateTime.Today.AddHours(20).AddMinutes(30);

            // doctor has no upcoming appointments.
            if (!appointments.Any())
            {
                // check close time
                if (appointmentFixingTime > appointmentCloseTime)
                {
                    // fix next day
                    appointmentFixingTime = DateTime.Today.AddDays(1).AddHours(9);
                    return appointmentFixingTime;
                }

                return appointmentFixingTime;
            }

            // get the latest appointment
            Appointment appointment = appointments.Last();
            DateTime appointmentEndtime = appointment.FixedDateTime.AddMinutes(30);
            appointmentFixingTime = appointmentEndtime.AddMinutes(15);

            if (appointment.FixedDateTime.Hour < 19)
            {
                return appointmentFixingTime;
            }
            else if (appointment.FixedDateTime.Hour == 19
                && appointmentEndtime <= appointmentCloseTime.AddMinutes(-15))
            {
                return appointmentFixingTime;
            }

            throw new DoctorAppointmentOverflowException();
        }

        public async Task<Appointment> BookAppointment(NewAppointmentDTO newAppointmentDTO)
        {
            Patient patient = await _patientRepository.Get(newAppointmentDTO.PatientId);
            Doctor doctor = await _doctorRepository.Get(newAppointmentDTO.DoctorId);
            IEnumerable<Appointment> appointments = new List<Appointment>();

            try
            {
                appointments = await _appointmentRepository.GetAll();
                // patient conflicts
                CheckPatientConflicts(appointments, newAppointmentDTO.PatientId);

                // check conflicts
                //CheckAppointmentConflicts(appointments, newAppointmentDTO.FixedDateTime, newAppointmentDTO.DoctorId);
            } catch (NoEntitiesAvailableException) { }


            Appointment appointment = _mapper.Map<Appointment>(newAppointmentDTO);
            appointment.Patient = patient;
            appointment.Doctor = doctor;
            appointment.FixedDateTime = GetAppointmentFixingTime(appointments, newAppointmentDTO.DoctorId);

            appointment = await _appointmentRepository.Create(appointment);

            return appointment;
        }

        public async Task<IEnumerable<DoctorResponseDTO>> GetAvailableDoctors(DateTime appointmentFixingDateTime)
        {
            var doctors = await _doctorRepository.GetAll();
            var appointments = (await _appointmentRepository.GetAll())
                .Where(a => a.FixedDateTime >= appointmentFixingDateTime);

            foreach (var doctor in doctors)
            {
                var doctorAppointments = appointments.Where(a => a.Doctor.Id ==  doctor.Id);  

                foreach (var appointment in doctorAppointments)
                {
                    DateTime appointmentEndTime = appointment.FixedDateTime.AddMinutes(30);

                    if (appointmentFixingDateTime > appointment.FixedDateTime
                        && appointmentFixingDateTime < appointmentEndTime)
                    {

                    }
                }
            }

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAppointments()
        {
            try
            {
                var appointments = (await _appointmentRepository.GetAll())
                .Where(a => a.AppointmentStatus == AppointmentStatus.Fixed);

                return appointments;
            } catch (NoEntitiesAvailableException)
            {
                return new List<Appointment>();
            }
        }
    }
}
