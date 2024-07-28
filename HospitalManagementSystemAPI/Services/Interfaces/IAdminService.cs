using HospitalManagementSystemAPI.DTOs.Doctor;
using HospitalManagementSystemAPI.DTOs.Staff;
using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IAdminService
    {
        Task<Staff> AddStaff(NewStaffDTO newStaffDTO);

        Task<Staff> AddStaff(NewDoctorDTO newDoctorDTO);
    }
}
