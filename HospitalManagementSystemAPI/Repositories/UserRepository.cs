using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;

namespace HospitalManagementSystemAPI.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(HospitalManagementSystemContext context) : base(context, "User") { }
    }
}
