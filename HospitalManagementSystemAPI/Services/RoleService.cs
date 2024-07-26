using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;

namespace HospitalManagementSystemAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;

        public RoleService(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _roleRepository.GetAll();
        }
    }
}
