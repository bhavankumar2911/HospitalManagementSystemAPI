﻿using HospitalManagementSystemAPI.DTOs.Staff;
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
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Staff> _staffRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public AdminService(IRepository<Role> roleRepository, IRepository<Staff> staffRepository, IAuthenticationService authenticationService, IRepository<User> userRepository)
        {
            _roleRepository = roleRepository;
            _staffRepository = staffRepository;
            _authenticationService = authenticationService;
            _userRepository = userRepository;
        }

        private async Task<Role> SaveRole(string roleName)
        {
            Role newRole = new Role(roleName.ToLower());
            await _roleRepository.Create(newRole);
            return newRole;
        }

        public async Task<Role> AddNewRole(string roleName)
        {
            // validation
            if (roleName == string.Empty) throw new InvalidRoleNameException();

            try
            {
                // check duplication
                var roles = await _roleRepository.GetAll();

                if (roles.Any(r => r.RoleName == roleName.ToLower())) throw new RoleDuplicationException(roleName);

                return await SaveRole(roleName);
            }
            catch (NoEntitiesAvailableException)
            {
                // if no roles present
                return await SaveRole(roleName);
            }
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
                DateOfBirth = newStaffDTO.DateOfBirth
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

        public async Task<Staff> AddStaff(NewStaffDTO newStaffDTO)
        {
            try
            {
                // get role
                Role role = await _roleRepository.Get(newStaffDTO.RoleId);

                // save staff
                Staff staff = await _staffRepository.Create(MapStaff(newStaffDTO));

                // hash password
                HMACSHA512 hMACSHA512 = new HMACSHA512();
                byte[] passwordHashKey = _authenticationService.GetHashKey(hMACSHA512);
                byte[] hashedPassword = _authenticationService.GetHashedPassword(hMACSHA512, newStaffDTO.PlainTextPassword);

                try
                {
                    // save user
                    User user = await _userRepository.Create(MapUser(staff, role, passwordHashKey, hashedPassword));

                    return staff;
                }
                catch (EntityCreationException)
                {
                    // delete the saved staff
                    await _staffRepository.Delete(staff.Id);
                    throw;
                }
            }
            catch (EntityNotFoundException)
            {
                throw new InvalidStaffInputException("The selected role is invalid.");
            }
        }
    }
}
