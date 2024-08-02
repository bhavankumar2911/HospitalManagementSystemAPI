using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Models;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystemAPI.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private readonly HospitalManagementSystemContext _context;

        public UserRepository(HospitalManagementSystemContext context) : base(context, "User")
        {
            _context = context;
        }


        public override async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Set<User>().Include(d => d.Staff).ToListAsync();

            if (users.Count > 0) return users;

            throw new NoEntitiesAvailableException("User");
        }

        public override async Task<User> Get(int id)
        {
            var user = await _context.Set<User>()
                .Include(d => d.Staff)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null) return user;

            throw new EntityNotFoundException("User", id);
        }
    }
}
