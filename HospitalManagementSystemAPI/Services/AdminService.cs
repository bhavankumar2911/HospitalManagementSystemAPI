using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Exceptions.Role;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;

namespace HospitalManagementSystemAPI.Services
{
    public class AdminService : IAdminService
    {
        public readonly IRepository<Role> _roleRepository;

        public AdminService(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        private async Task<Role> SaveRole(string roleName)
        {
            Role newRole = new Role(roleName.ToLower());
            await _roleRepository.Create(newRole);
            return newRole;
        }

        public async Task<Role> AddNewRole(string roleName)
        {
            // validation
            if (roleName == string.Empty) throw new InvalidRoleNameException();

            try
            {
                // check duplication
                var roles = await _roleRepository.GetAll();

                if (roles.Any(r => r.RoleName == roleName.ToLower())) throw new RoleDuplicationException(roleName);

                return await SaveRole(roleName);
            }
            catch (NoEntitiesAvailableException)
            {
                // if no roles present
                return await SaveRole(roleName);
            }
        }
    }
}
