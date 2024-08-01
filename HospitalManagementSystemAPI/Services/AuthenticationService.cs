using AutoMapper;
using HospitalManagementSystemAPI.DTOs.Authentication;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Exceptions.Authentication;
using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Objects.Authentication;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagementSystemAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Staff> _staffRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthenticationService(IRepository<User> userRepository, IRepository<Staff> staffRepository, ITokenService tokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _staffRepository = staffRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public bool ComparePassword(byte[] passwordFromUser, byte[] passwordInDB)
        {
            if (passwordFromUser.Length != passwordInDB.Length) return false;

            for (int i = 0; i < passwordFromUser.Length; i++)
            {
                if (passwordFromUser[i] != passwordInDB[i])
                {
                    return false;
                }
            }
            return true;
        }

        private byte[] GetHashedPassword(HMACSHA512 hMACSHA, string plainTextPassword)
        {
            return hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
        }

        private byte[] GetHashKey(HMACSHA512 hMACSHA)
        {
            return hMACSHA.Key;
        }

        public PasswordInformation GetPasswordInformation(string plainTextPassword)
        {
            HMACSHA512 hMACSHA512 = new HMACSHA512();
            byte[] passwordHashKey = GetHashKey(hMACSHA512);
            byte[] hashedPassword = GetHashedPassword(hMACSHA512, plainTextPassword);

            return new PasswordInformation { PasswordHashKey = passwordHashKey, HashedPassword = hashedPassword };
        }

        public async Task<LoginResponseDTO> LoginStaff(StaffLoginInputDTO staffLoginInputDTO)
        {
            var user = (await _userRepository.GetAll())
                    .Where(u => u.Email == staffLoginInputDTO.Email);

            if (!user.Any()) throw new InvalidLoginCredentialsException();

            // check password
            HMACSHA512 hMACSHA512 = new HMACSHA512(user.First().PasswordHashKey);
            byte[] hashedPassword = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(staffLoginInputDTO.PlainTextPassword));

            if (ComparePassword(hashedPassword, user.First().HashedPassword))
            {
                var staff = await _staffRepository.Get(user.First().Staff.Id);

                LoginResponseDTO loginResponseDTO = _mapper.Map<LoginResponseDTO>(staff);
                loginResponseDTO.Token = _tokenService
                    .GenerateToken(
                        user.First().Id,
                        user.First().Role
                    );
                loginResponseDTO.Role = user.First().Role;

                return loginResponseDTO;    
            }

            throw new InvalidLoginCredentialsException();
        }
    }
}
