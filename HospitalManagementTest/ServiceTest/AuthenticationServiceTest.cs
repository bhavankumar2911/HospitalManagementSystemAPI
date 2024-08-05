using AutoMapper;
using HospitalManagementSystemAPI;
using HospitalManagementSystemAPI.DTOs.Authentication;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Enums;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Profiles;
using HospitalManagementSystemAPI.Repositories;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services;
using HospitalManagementSystemAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementTest.ServiceTest
{
    internal class AuthenticationServiceTest
    {
        private HospitalManagementSystemContext _context;
        private IRepository<User> _userRepository;
        private IRepository<Staff> _staffRepository;
        private IRepository<Nurse> _nurseRepository;
        private IRepository<Appointment> _appointmentRepository;
        private IRepository<Doctor> _doctorRepository;

        private ITokenService _tokenService;
        private INurseService _nurseService;
        private IDoctorService _doctorService;
        private IAuthenticationService _authenticationService;
        private IAdminService _adminService;

        private IMapper _mapper;
        private IMapper _doctorMapper;

        [SetUp]
        public async Task Setup()
        {
            //config
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");

            Mock<IConfigurationSection> congigTokenSection = new Mock<IConfigurationSection>();
            congigTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);

            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);

            // context
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("dummyDB");
            _context = new HospitalManagementSystemContext(optionsBuilder.Options);

            // repos
            _userRepository = new UserRepository(_context);
            _staffRepository = new StaffRepository(_context);
            _nurseRepository = new NurseRepository(_context);
            _doctorRepository = new DoctorRepository(_context);
            _appointmentRepository = new AppointmentRepository(_context);

            var staffProfile = new StaffProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(staffProfile));
            _mapper = new Mapper(configuration);
            var doctorProfile = new DoctorProfile();
            var doctorConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(doctorProfile));
            _doctorMapper = new Mapper(doctorConfiguration);

            // service
            _nurseService = new NurseService(_nurseRepository);
            _doctorService = new DoctorService(_doctorRepository, _doctorMapper, _appointmentRepository);
            _tokenService = new TokenService(mockConfig.Object);
            _authenticationService = new AuthenticationService(
                    _userRepository,
                    _staffRepository,
                    _tokenService,
                    _mapper
                );
            _adminService = new AdminService(
                    _staffRepository,
                    _authenticationService,
                    _userRepository,
                    _nurseService,
                    _doctorService,
                    _mapper
                );

            // login check setup
            NewStaffDTO newStaffDTO = new NewStaffDTO
            {
                Email = "test@gmail.com",
                Phone = "+1 123456782",
                Role = Role.Receptionist,
                Firstname = "John",
                PlainTextPassword = "password",
                DateOfBirth = DateTime.Now,
                DateOfJoining = DateTime.Now,
                Address = "India"
            };

            await _adminService.AddStaff(newStaffDTO);
        }

        [Test]
        public void GetPasswordInformationTest ()
        {
            var passwordInformation = _authenticationService.GetPasswordInformation("password123");

            Assert.Multiple(() =>
            {
                Assert.That(passwordInformation.PasswordHashKey, Is.Not.Null);
                Assert.That(passwordInformation.HashedPassword, Is.Not.Null);
            });
        }

        [Test]
        public void ComparePasswordTest()
        {
            var passwordInfo = _authenticationService.GetPasswordInformation("password");
            HMACSHA512 hMACSHA512 = new HMACSHA512(passwordInfo.PasswordHashKey);
            byte[] passwordFromUser = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes("password"));

            Assert.That(_authenticationService.ComparePassword(passwordFromUser, passwordInfo.HashedPassword), Is.True);
        }

        [Test]
        public async Task LoginStaffTest ()
        {
            LoginResponseDTO loginResponseDTO = await _authenticationService.LoginStaff(new StaffLoginInputDTO
            {
                Email = "test@gmail.com",
                PlainTextPassword = "password"
            });

            Assert.That(loginResponseDTO.Token, Is.Not.Null);
        }
    }
}
