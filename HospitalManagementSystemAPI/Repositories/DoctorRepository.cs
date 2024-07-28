using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>
    {
        public DoctorRepository(HospitalManagementSystemContext context) : base(context, "Doctor")
        {
        }
    }
}
