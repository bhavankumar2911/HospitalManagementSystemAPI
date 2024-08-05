using HospitalManagementSystemAPI.Services.Interfaces;
using HospitalManagementSystemAPI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using HospitalManagementSystemAPI.Enums;

namespace HospitalManagementTest.ServiceTest
{
    internal class TokenServiceTest
    {
        private ITokenService _tokenService;

        [SetUp]
        public void SetUp()
        {
            // config
            Mock<IConfigurationSection> configurationJWTSection = new Mock<IConfigurationSection>();
            configurationJWTSection.Setup(x => x.Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");

            Mock<IConfigurationSection> congigTokenSection = new Mock<IConfigurationSection>();
            congigTokenSection.Setup(x => x.GetSection("JWT")).Returns(configurationJWTSection.Object);

            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection("TokenKey")).Returns(congigTokenSection.Object);

            _tokenService = new TokenService(mockConfig.Object);
        }

        [Test]
        public void CreateTokenPassTest()
        {
            string token = _tokenService.GenerateToken(1, Role.Doctor);

            Assert.IsNotNull(token);
        }
    }
}
