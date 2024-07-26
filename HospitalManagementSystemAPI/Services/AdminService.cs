using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Enums;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Role;
using HospitalManagementSystemAPI.Exceptions.Staff;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HospitalManagementSystemAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Staff> _staffRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public AdminService(IRepository<Staff> staffRepository, IAuthenticationService authenticationService, IRepository<User> userRepository)
        {
            _staffRepository = staffRepository;
            _authenticationService = authenticationService;
            _userRepository = userRepository;
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

        private async Task CheckEmailPhoneDuplication (NewStaffDTO newStaffDTO)
        {
            // email check
            try
            {
                var users = await _userRepository.GetAll();

                if (users.Any(user => user.Email == newStaffDTO.Email)) throw new StaffEmailDuplicationException();
            }
            catch (NoEntitiesAvailableException) { }

            // phone check
            try
            {
                var staffs = await _staffRepository.GetAll();

                if (staffs.Any(staff => staff.Phone == newStaffDTO.Phone)) throw new StaffPhoneDuplicationException();
            }
            catch (NoEntitiesAvailableException) { }
        }

        public async Task<Staff> AddStaff(NewStaffDTO newStaffDTO)
        {
            // check email and phone number duplication
            await CheckEmailPhoneDuplication(newStaffDTO);

            // save staff
            Staff staff = await _staffRepository.Create(MapStaff(newStaffDTO));

            // hash password
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            byte[] passwordHashKey = _authenticationService.GetHashKey(hMACSHA512);
            byte[] hashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, newStaffDTO.PlainTextPassword);

            try
            {
                // save user
                User user = await _userRepository.Create(MapUser(staff, newStaffDTO.Role, passwordHashKey, hashedPassword));

                return staff;
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
