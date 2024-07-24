using HospitalManagementSystemAPI.Exceptions.Generic;
using HospitalManagementSystemAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Text;

namespace HospitalManagementSystemAPI.Repositories
{
    public abstract class BaseRepository<Entity> : IRepository<Entity> where Entity : class
    {
        private readonly HospitalManagementSystemContext _context;
        private readonly string _entityName;

        public BaseRepository(HospitalManagementSystemContext context, string entityName)
        {
            _context = context;
            _entityName = entityName;
        }

        public virtual async Task<Entity> Create(Entity entity)
        {
            try
            {
                _context.Set<Entity>().Add(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch
            {
                throw new EntityCreationException(_entityName);
            }
        }

        public virtual async Task<bool> Delete(int id)
        {
            try
            {
                var entity = await Get(id);

                _context.Remove(entity);
                await _context.SaveChangesAsync(true);

                return true;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch
            {
                throw new EntityDeletionException(_entityName, id);
            }
        }

        public virtual async Task<Entity> Get(int id)
        {
            var entity = await _context.Set<Entity>().FindAsync(id);

            if (entity !=  null) return entity;

            throw new EntityNotFoundException(_entityName, id);
        }

        public virtual async Task<IEnumerable<Entity>> GetAll()
        {
            var entities = await _context.Set<Entity>().ToListAsync();  

            if (entities.Count > 0) return entities;

            throw new NoEntitiesAvailableException(_entityName);
        }

        public virtual async Task<Entity> Update(Entity updatedEntity, int id)
        {
            try
            {
                var entity = await Get(id);

                _context.Update(entity);
                await _context.SaveChangesAsync(true);

                return entity;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch
            {
                throw new EntityUpdationException(_entityName, id);
            }
        }
    }
}
