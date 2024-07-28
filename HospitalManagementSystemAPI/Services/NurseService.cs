using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using HospitalManagementSystemAPI.Services.Interfaces;

namespace HospitalManagementSystemAPI.Services
{
    public class NurseService : INurseService
    {
        private readonly IRepository<Nurse> _nurseRepository;

        public NurseService(IRepository<Nurse> nurseRepository)
        {
            _nurseRepository = nurseRepository;
        }

        public async Task SaveNewNurse(Nurse nurse)
        {
            await _nurseRepository.Create(nurse);
        }
    }
}
