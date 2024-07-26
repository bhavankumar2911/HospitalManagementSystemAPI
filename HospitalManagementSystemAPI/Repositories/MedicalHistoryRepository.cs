using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Repositories
{
    public class MedicalHistoryRepository : BaseRepository<MedicalHistory>
    {
        public MedicalHistoryRepository(HospitalManagementSystemContext context) : base(context, "Medical history")
        {
        }
    }
}
