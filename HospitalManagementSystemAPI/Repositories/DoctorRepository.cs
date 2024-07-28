using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemAPI.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>
    {
        private readonly HospitalManagementSystemContext _context;

        public DoctorRepository(HospitalManagementSystemContext context) : base(context, "Doctor")
        {
            _context = context;
        }

        public override async Task<IEnumerable<Doctor>> GetAll()
        {
            var entities = await _context.Set<Doctor>().Include(d => d.Staff).ToListAsync();

            if (entities.Count > 0) return entities;

            throw new NoEntitiesAvailableException("Doctor");
        }
    }
}
