using AutoMapper;
using HospitalManagementSystemAPI;
using HospitalManagementSystemAPI.DTOs.Doctor;
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

namespace HospitalManagementTest.ServiceTest
{
    internal class AdminServiceTest
    {
        private HospitalManagementSystemContext _context;
        private IAdminService _adminService;
        private ITokenService _tokenService;
        private IRepository<Staff> _staffRepository;
        private IRepository<Nurse> _nurseRepository;
        private IRepository<User> _userRepository;
        private IRepository<Doctor> _doctorRepository;
        private IRepository<Appointment> _appointmentRepository;
        private IAuthenticationService _authenticationService;
        private INurseService _nurseService;
        private IDoctorService _doctorService;
        private IMapper _mapper;
        private IMapper _doctorMapper;

        [SetUp]
        public void Setup ()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder().UseInMemoryDatabase("dummyDB");
            _context = new HospitalManagementSystemContext(optionsBuilder.Options);

            // config
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");

            Mock<IConfigurationSection> congigTokenSection = new Mock<IConfigurationSection>();
            congigTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);

            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);

            _tokenService = new TokenService(mockConfig.Object);

            _staffRepository = new StaffRepository(_context);
            _userRepository = new UserRepository(_context);
            _nurseRepository = new NurseRepository(_context);
            _doctorRepository = new DoctorRepository(_context);
            _appointmentRepository = new AppointmentRepository(_context);

            var staffProfile = new StaffProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(staffProfile));
            _mapper = new Mapper(configuration);

            var doctorProfile = new DoctorProfile();
            var doctorConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(doctorProfile));
            _doctorMapper = new Mapper(doctorConfiguration);

            _nurseService = new NurseService(_nurseRepository);
            _doctorService = new DoctorService(_doctorRepository, _doctorMapper, _appointmentRepository);

            _authenticationService = new AuthenticationService(_userRepository, _staffRepository, _tokenService, _mapper);

            _adminService = new AdminService(
                    _staffRepository,
                    _authenticationService,
                    _userRepository,
                    _nurseService,
                    _doctorService,
                    _mapper
                );
        }

        [Test]
        public async Task AddStaffTest()
        {
            NewStaffDTO newStaffDTO = new NewStaffDTO
            {
                Email = "john@gmail.com",
                Phone = "+1 123456789",
                Role = Role.Receptionist,
                Firstname = "John",
                PlainTextPassword = "password",
                DateOfBirth = DateTime.Now,
                DateOfJoining = DateTime.Now,
                Address = "India"
            };

            var staff = await _adminService.AddStaff(newStaffDTO);

            Assert.Multiple(() =>
            {
                Assert.That(_context.Staffs.Count, Is.EqualTo(1));
                Assert.That(_context.Users.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task AddDoctorTest()
        {
            NewDoctorDTO newDoctor = new NewDoctorDTO
            {
                Email = "john1@gmail.com",
                Phone = "+1 123456787",
                Role = Role.Doctor,
                Firstname = "John",
                PlainTextPassword = "password",
                DateOfBirth = DateTime.Now,
                DateOfJoining = DateTime.Now,
                Address = "India",
                Qualification = "MBBS",
                Specialization = "Dentist"
            };

            var staff = await _adminService.AddStaff(newDoctor);

            Assert.Multiple(() =>
            {
                Assert.That(_context.Staffs.Count, Is.EqualTo(1));
                Assert.That(_context.Users.Count, Is.EqualTo(1));
                Assert.That(_context.Doctors.Count, Is.EqualTo(1));
            });
        }
    }
}
