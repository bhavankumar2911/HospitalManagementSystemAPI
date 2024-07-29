using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.Exceptions.Generic;
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

                return result;
            } catch (NoEntitiesAvailableException)
            {
                return await ViewAllDoctors();
            }
        }

        //public async Task<IEnumerable<DoctorResponseDTO>> GetDoctorsWithLeastAppointments()
        //{
        //    var doctors = await _doctorRepository.GetAll();

        //    try
        //    {
        //        var appointments = await _appointmentRepository.GetAll();

        //        var doctorAppointments = doctors
        //            .GroupJoin(
        //                appointments,
        //                doctor => doctor.Id,
        //                appointment => appointment.Doctor.Id,
        //                (doctor, appointmentGroup) => new { doctor, appointmentGroup }
        //            )
        //            .SelectMany(
        //                x => x.appointmentGroup.DefaultIfEmpty(),
        //                (x, appointment) =>
        //                {
        //                    return new
        //                    {
        //                        Appointment = appointment,
        //                        Doctor = x.doctor,
        //                    };
        //                }
        //            );

        //        var orderedDoctors = doctorAppointments
        //        .GroupBy(appointment => appointment.Doctor)
        //        .OrderBy(group => group.Count())
        //        .Select(group =>
        //        {
        //            DoctorResponseDTO doctorResponseDTO = _mapper.Map<DoctorResponseDTO>(group.Key.Staff);
        //            doctorResponseDTO = _mapper.Map<Doctor, DoctorResponseDTO>(group.Key, doctorResponseDTO);

        //            return doctorResponseDTO;
        //        });

        //        foreach (var item in orderedDoctors)
        //        {
        //            Console.WriteLine( item.Firstname + " " + item.Lastname + " " + item.Id );
        //        }

        //        return await ViewAllDoctors();
        //    } catch (NoEntitiesAvailableException)
        //    {
        //        return await ViewAllDoctors();
        //    }
        //}

        //public async Task<IEnumerable<DoctorResponseDTO>> GetDoctorsWithLeastAppointments()
        //{
        //    var doctors = await ViewAllDoctors();

        //    try
        //    {
        //        var orderedDoctors = (await _appointmentRepository.GetAll())
        //        .GroupBy(appointment => appointment.Doctor)
        //        .OrderBy(group => group.Count())
        //        .Select(group =>
        //        {
        //            DoctorResponseDTO doctorResponseDTO = _mapper.Map<DoctorResponseDTO>(group.Key.Staff);
        //            doctorResponseDTO = _mapper.Map<Doctor, DoctorResponseDTO>(group.Key, doctorResponseDTO);

        //            return doctorResponseDTO;
        //        });

        //        // add doctors without any appointments
        //        foreach (var doctor in doctors)
        //        {
        //            foreach (var orderedDoctor in orderedDoctors)
        //            {
        //                if (doctor.Id == orderedDoctor.Id)
        //            }
        //        }

        //        return orderedDoctors;
        //    } catch (NoEntitiesAvailableException)
        //    {
        //        return 
        //    }
        //}

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
