using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FoodOnline.DataAccess.Repository
{
    public class BaseRepository<T> where T : class, IAuditedEntityBase
    {


        private readonly FoodDbContext _context;



        public BaseRepository(FoodDbContext context)
        {
            _context = context;

        }






        //get all entity with condition and include
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            query = query.Where(q => q.IsDeleted != true);

            if (includeProperties.Any())
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }



            return await query.ToListAsync();
        }

        //get 1 entity with condition and include
        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
            query = query.Where(q => q.IsDeleted != true);
            // Apply the filter expression
            query = query.Where(expression);

            // Apply includes for the specified navigation properties
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            // Return the single entity or null if not found
            return await query.SingleOrDefaultAsync();
        }


        //add record to db
        public async Task Create(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            await _context.Set<T>().AddAsync(entity);

        }


        //add multi record to db
        public async Task CreateList(List<T> entities)
        {

            foreach (var entity in entities)
            {
                entity.CreatedDate = DateTime.Now;
            }


            await _context.Set<T>().AddRangeAsync(entities);

        }

        //update entity
        public void Update(T entity)
        {
            entity.LastModifiedDate = DateTime.Now;
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        //update multiple entities
        public void UpdateList(List<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.LastModifiedDate = DateTime.Now;
                _context.Set<T>().Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
        }



        //mark as delete
        public void MarkAsDelete(T entity)
        {
            entity.LastModifiedDate = DateTime.Now;
            entity.IsDeleted = true;
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }



        //delete entity
        public void Delete(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void DeleteList(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }



        //save change
        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }


    }
}
