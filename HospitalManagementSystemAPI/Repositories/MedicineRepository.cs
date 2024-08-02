using HospitalManagementSystemAPI.Models;

namespace HospitalManagementSystemAPI.Repositories
{
    public class MedicineRepository : BaseRepository<Medicine>
    {
        public MedicineRepository(HospitalManagementSystemContext context) : base(context, "Medicine")
        {
        }
    }
}
