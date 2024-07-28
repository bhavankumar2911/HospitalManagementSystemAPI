using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Enums;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Staff;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Objects.Authentication;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;

namespace HospitalManagementSystemAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Staff> _staffRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly INurseService _nurseService;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;

        public AdminService(IRepository<Staff> staffRepository, IAuthenticationService authenticationService, IRepository<User> userRepository, INurseService nurseService, IDoctorService doctorService, IMapper mapper)
        {
            _staffRepository = staffRepository;
            _authenticationService = authenticationService;
            _userRepository = userRepository;
            _nurseService = nurseService;
            _doctorService = doctorService;
            _mapper = mapper;
        }

        private Staff MapStaff(NewStaffDTO newStaffDTO)
        {
            return new Staff
            {
                Firstname = newStaffDTO.Firstname,
                Lastname = newStaffDTO.Lastname,
                DateOfJoining = newStaffDTO.DateOfJoining,
                Email = newStaffDTO.Email,
                Address = newStaffDTO.Address,
                DateOfBirth = newStaffDTO.DateOfBirth,
                Phone = newStaffDTO.Phone
            };
        }

        private User MapUser(Staff staff, Role role, byte[] passwordHashKey, byte[] hashedPassword)
        {
            return new User
            {
                Email = staff.Email,
                Role = role,
                Staff = staff,
                PasswordHashKey = passwordHashKey,
                HashedPassword = hashedPassword
            };
        }

        private async Task CheckEmailPhoneDuplication (string email, string phone)
        {
            // email check
            try
            {
                var users = await _userRepository.GetAll();

                if (users.Any(user => user.Email == email)) throw new StaffEmailDuplicationException();
            }
            catch (NoEntitiesAvailableException) { }

            // phone check
            try
            {
                var staffs = await _staffRepository.GetAll();

                if (staffs.Any(staff => staff.Phone == phone)) throw new StaffPhoneDuplicationException();
            }
            catch (NoEntitiesAvailableException) { }
        }

        private async Task SaveStaffSeparately (NewStaffDTO newStaffDTO, Staff staff)
        {
            switch (newStaffDTO.Role)
            {
                case Role.Nurse:
                    Nurse nurse = new() { Staff = staff };
                    await _nurseService.SaveNewNurse(nurse);
                    break;
                case Role.Doctor:
                    throw new InvalidStaffInputException("Invalid role.");
            }
        }

        // save to staff and other tables based on role except doctor
        public async Task<Staff> AddStaff(NewStaffDTO newStaffDTO)
        {
            // check email and phone number duplication
            await CheckEmailPhoneDuplication(newStaffDTO.Email, newStaffDTO.Phone);

            // save staff
            Staff staff = await _staffRepository.Create(MapStaff(newStaffDTO));

            // hash password
            PasswordInformation passwordInformation = _authenticationService.GetPasswordInformation(newStaffDTO.PlainTextPassword);

            try
            {
                // save user
                User user = await _userRepository.Create(MapUser(staff, newStaffDTO.Role, passwordInformation.PasswordHashKey, passwordInformation.HashedPassword));

                var users = await _userRepository.GetAll();

                foreach (var item in users)
                {
                    Console.Out.WriteLine(item.Id);
                }

                try
                {
                    await SaveStaffSeparately(newStaffDTO, staff);

                    return staff;
                }
                catch (Exception)
                {
                    // delete the saved staff and user
                    await _staffRepository.Delete(staff.Id);

                    throw;
                }
            }
            catch (EntityCreationException)
            {
                // delete the saved staff
                await _staffRepository.Delete(staff.Id);
                throw;
            }
        }

        // save to staff and doctor table
        public async Task<Staff> AddStaff(NewDoctorDTO newDoctorDTO)
        {
            // check role
            if (newDoctorDTO.Role != Role.Doctor) throw new InvalidStaffInputException("Invalid role.");

            // check email and phone number duplication
            await CheckEmailPhoneDuplication(newDoctorDTO.Email, newDoctorDTO.Phone);

            // save staff
            Staff staff = await _staffRepository.Create(_mapper.Map<Staff>(newDoctorDTO));

            // hash password
            PasswordInformation passwordInformation = _authenticationService.GetPasswordInformation(newDoctorDTO.PlainTextPassword);

            try
            {
                // save user
                User user = await _userRepository.Create(MapUser(staff, newDoctorDTO.Role, passwordInformation.PasswordHashKey, passwordInformation.HashedPassword));

                try
                {
                    Doctor doctor = new()
                    {
                        Qualification = newDoctorDTO.Qualification,
                        Specialization = newDoctorDTO.Specialization,
                        Staff = staff
                    };
                    await _doctorService.AddNewDoctor(doctor);

                    return staff;
                }
                catch (EntityCreationException)
                {
                    // delete the saved staff
                    await _staffRepository.Delete(staff.Id);

                    throw;
                }
            }
            catch (EntityCreationException)
            {
                // delete the saved staff
                await _staffRepository.Delete(staff.Id);
                throw;
            }
        }
    }
}
