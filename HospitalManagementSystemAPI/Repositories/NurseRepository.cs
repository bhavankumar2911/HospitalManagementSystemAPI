using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Repositories
{
    public class NurseRepository : BaseRepository<Nurse>
    {
        public NurseRepository(HospitalManagementSystemContext context) : base(context, "Nurse")
        {
        }
    }
}
