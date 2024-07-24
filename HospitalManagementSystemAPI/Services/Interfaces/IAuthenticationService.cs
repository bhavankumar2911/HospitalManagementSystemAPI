using HospitalManagementSystemAPI.Models;
using System.Security.Cryptography;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public byte[] GetHashKey(HMACSHA512 hMACSHA);

        public byte[] GetHashedPassword(HMACSHA512 hMACSHA, string plainTextPassword);

        public bool ComparePassword(byte[] passwordFromUser, byte[] passwordInDB);
    }
}
