using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Repositories
{
    public class PrescriptionRepository : BaseRepository<Prescription>
    {
        public PrescriptionRepository(HospitalManagementSystemContext context) : base(context, "Prescription")
        {
        }
    }
}
