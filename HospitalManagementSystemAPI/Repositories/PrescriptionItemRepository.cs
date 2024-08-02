using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Repositories
{
    public class PrescriptionItemRepository : BaseRepository<PrescriptionItem>
    {
        public PrescriptionItemRepository(HospitalManagementSystemContext context) : base(context, "Prescription item")
        {
        }
    }
}
