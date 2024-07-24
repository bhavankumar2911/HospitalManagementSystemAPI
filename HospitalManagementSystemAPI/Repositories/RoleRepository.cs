using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Repositories
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(HospitalManagementSystemContext context) : base(context, "Role") { }
    }
}
