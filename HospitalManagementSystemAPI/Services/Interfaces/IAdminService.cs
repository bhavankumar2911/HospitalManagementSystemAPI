using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IAdminService
    {
        Task<Role> AddNewRole(string roleName);

        Task<Staff> AddStaff(NewStaffDTO newStaffDTO);
    }
}
