using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Objects.Authentication;
using HospitalManagementSystemAPI.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagementSystemAPI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
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
    }
}
