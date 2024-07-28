using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface INurseService
    {
        public Task SaveNewNurse(Nurse nurse);
    }
}
