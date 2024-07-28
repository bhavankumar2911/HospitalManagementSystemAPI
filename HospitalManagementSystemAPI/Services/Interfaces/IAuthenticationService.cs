using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Objects.Authentication;
using System.Security.Cryptography;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public bool ComparePassword(byte[] passwordFromUser, byte[] passwordInDB);

        public PasswordInformation GetPasswordInformation(string plainTextPassword);
    }
}
