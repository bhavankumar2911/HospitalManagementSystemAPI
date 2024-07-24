using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IAdminService
    {
        Task<Role> AddNewRole(string roleName);
    }
}
