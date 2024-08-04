using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Appointment;
using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Migrations;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Numerics;

namespace HospitalManagementSystemAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IMapper _mapper;

        public DoctorService(IRepository<Doctor> doctorRepository, IMapper mapper, IRepository<Appointment> appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _appointmentRepository = appointmentRepository;
        }

        public async Task AddNewDoctor(Doctor doctor)
        {
            await _doctorRepository.Create(doctor);
        }

        private DoctorAppointmentSlotsDTO GetAllSlots (
                int[] appointmentStartHours,
                int[] appointmentStartMinutes
            )
        {
            IList<SlotDTO> slots = new List<SlotDTO>();

            for (int i = 0; i < appointmentStartHours.Length; i++)
            {
                int hours = appointmentStartHours[i];
                int minutes = appointmentStartMinutes[i];
                DateTime newSlotTime = DateTime.Today.AddHours(hours).AddMinutes(minutes);
                DateTime currentTime = DateTime.Now;

                if (newSlotTime > currentTime)
                    slots.Add(new SlotDTO
                    {
                        From = newSlotTime,
                        To = newSlotTime.AddMinutes(30),
                    });
            }

            return new DoctorAppointmentSlotsDTO { AppointmentSlots = slots };
        }

        public async Task<DoctorAppointmentSlotsDTO> GetDoctorAppointmentSlots(int doctorId)
        {

            int[] appointmentStartHours = { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            int[] appointmentStartMinutes = { 0, 0, 0, 0, 30, 30, 30, 30, 30, 30 };

            List<TimeOnly> slots = new List<TimeOnly>();

            for (int i = 0; i < appointmentStartHours.Length; i++)
            {
                TimeOnly slotFrom = new TimeOnly()
                    .AddHours(appointmentStartHours[i])
                    .AddMinutes(appointmentStartMinutes[i]);

                if (slotFrom > TimeOnly.FromDateTime(DateTime.Now))
                    slots.Add(slotFrom);
            }

            try
            {
                var appointments = (await _appointmentRepository.GetAll())
                .Where(
                    a => a.Doctor.Id == doctorId
                    && DateOnly.FromDateTime(a.FixedDateTime) == DateOnly.FromDateTime(DateTime.Today)
                    && a.AppointmentStatus == Enums.AppointmentStatus.Fixed
                );

                if (!appointments.Any() )
                    return GetAllSlots(appointmentStartHours, appointmentStartMinutes);

                foreach (var appointment in appointments)
                {
                    slots.Remove(TimeOnly.FromDateTime(appointment.FixedDateTime));
                }

                var appointmentSlots = new List<SlotDTO>();

                foreach (var slot in slots)
                {
                    appointmentSlots.Add(new SlotDTO
                    {
                        From = DateTime.Today.AddHours(slot.Hour).AddMinutes(slot.Minute),
                        To = DateTime.Today.AddHours(slot.Hour).AddMinutes(slot.Minute + 30),
                    });
                }

                return new DoctorAppointmentSlotsDTO { AppointmentSlots = appointmentSlots };
            } catch (NoEntitiesAvailableException)
            {
                return GetAllSlots(appointmentStartHours, appointmentStartMinutes);
            }
        }

        public async Task<IEnumerable<DoctorResponseDTO>> GetDoctorsWithLeastAppointments()
        {
            var doctors = await _doctorRepository.GetAll();

            try
            {
                var appointments = await _appointmentRepository.GetAll();
                var doctorAppointmentCount = new Dictionary<Doctor, int>();

                foreach (var appointment in appointments)
                {
                    if (!doctorAppointmentCount.ContainsKey(appointment.Doctor))
                        doctorAppointmentCount.Add( appointment.Doctor, 1);
                    else
                        doctorAppointmentCount[appointment.Doctor]++;
                }

                doctorAppointmentCount = doctorAppointmentCount.OrderBy(count => count.Value)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value
                    );
                var orderedDoctors = doctorAppointmentCount.Keys.ToList();

                var result = orderedDoctors
                .Select(doctor =>
                {
                    DoctorResponseDTO doctorResponseDTO = _mapper.Map<DoctorResponseDTO>(doctor.Staff);
                    doctorResponseDTO = _mapper.Map<Doctor, DoctorResponseDTO>(doctor, doctorResponseDTO);

                    return doctorResponseDTO;
                });

                // add doctors without any appointments
                foreach (var doctor in doctors)
                {
                    if (!doctorAppointmentCount.ContainsKey(doctor))
                    {
                        DoctorResponseDTO doctorResponseDTO = _mapper.Map<DoctorResponseDTO>(doctor.Staff);
                        doctorResponseDTO = _mapper.Map(doctor, doctorResponseDTO);

                        _ = result.Append(doctorResponseDTO);
                    }
                }

                return result;
            } catch (NoEntitiesAvailableException)
            {
                return await ViewAllDoctors();
            }
        }

        public async Task<IEnumerable<DoctorResponseDTO>> ViewAllDoctors()
        {
            var doctors = (await _doctorRepository.GetAll())
                .Select(doctor =>
                {
                    //DoctorResponseDTO doctorResponseDTO = _mapper.Map<DoctorResponseDTO>(doctor);
                    //doctorResponseDTO = _mapper.Map<Staff, DoctorResponseDTO>(doctor.Staff, doctorResponseDTO);

                    DoctorResponseDTO doctorResponseDTO = _mapper.Map<DoctorResponseDTO>(doctor.Staff);
                    doctorResponseDTO = _mapper.Map<Doctor, DoctorResponseDTO>(doctor, doctorResponseDTO);

                    return doctorResponseDTO;
                });

            return doctors;
        }
    }
}
