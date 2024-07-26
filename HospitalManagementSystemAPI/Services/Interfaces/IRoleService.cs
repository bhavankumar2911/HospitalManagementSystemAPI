using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Services.Interfaces
{
    public interface IRoleService
    {
        public Task<IEnumerable<Role>> GetRoles();
    }
}
