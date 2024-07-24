using HospitalManagementSystemAPI.Models;
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

        public byte[] GetHashedPassword(HMACSHA512 hMACSHA, string plainTextPassword)
        {
            return hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
        }

        public byte[] GetHashKey(HMACSHA512 hMACSHA)
        {
            return hMACSHA.Key;
        }
    }
}
