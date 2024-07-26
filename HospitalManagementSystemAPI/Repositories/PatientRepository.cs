using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Repositories
{
    public class PatientRepository : BaseRepository<Patient>
    {
        public PatientRepository(HospitalManagementSystemContext context) : base(context, "Patient")
        {
        }
    }
}
