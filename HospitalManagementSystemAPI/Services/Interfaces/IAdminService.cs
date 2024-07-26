using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IAdminService
    {
        Task<Staff> AddStaff(NewStaffDTO newStaffDTO);
    }
}
