using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Repositories
{
    public class StaffRepository : BaseRepository<Staff>
    {
        public StaffRepository(HospitalManagementSystemContext context) : base(context, "Staff") { }
    }
}
