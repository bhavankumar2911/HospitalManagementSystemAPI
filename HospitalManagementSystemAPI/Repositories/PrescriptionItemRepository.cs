using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemAPI.Repositories
{
    public class PrescriptionItemRepository : BaseRepository<PrescriptionItem>
    {
        private readonly HospitalManagementSystemContext _context;
        public PrescriptionItemRepository(HospitalManagementSystemContext context) : base(context, "Prescription item")
        {
            _context = context;
        }

        public override async Task<IEnumerable<PrescriptionItem>> GetAll()
        {
            var entities = await _context.Set<PrescriptionItem>()
                .Include(pi => pi.Medicine)
                .Include(pi => pi.Prescription)
                .ThenInclude(p => p.Patient)
                .Include(pi => pi.Prescription)
                .ThenInclude(p => p.Doctor)
                .ToListAsync();

            if (entities.Count > 0) return entities;

            throw new NoEntitiesAvailableException("Prescription item");
        }
    }
}
