using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemAPI.Repositories
{
    public class AppointmentRepository : BaseRepository<Appointment>
    {
        private readonly HospitalManagementSystemContext _context;

        public AppointmentRepository(HospitalManagementSystemContext context) : base(context, "Appointment")
        {
            _context = context;
        }

        public override async Task<IEnumerable<Appointment>> GetAll()
        {
            var appointments = await _context.Set<Appointment>().Include(d => d.Doctor.Staff).Include(a => a.Patient).ToListAsync();

            if (appointments.Count > 0) return appointments;

            throw new NoEntitiesAvailableException("Appointment");
        }
    }
}
