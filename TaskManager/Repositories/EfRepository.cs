using Microsoft.EntityFrameworkCore;

namespace TaskManager.Repositories
{
    public class EfRepository<T>(DbContext context) : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public Task<T?> GetByIdAsync(int id) => _dbSet.FirstOrDefaultAsync(e => e.Id == id);

        public Task<List<T>> GetAllAsync() => _dbSet.ToListAsync();

        public Task<List<T>> GetPagedAsync(int skip, int take) =>
              _dbSet
                .OrderByDescending(e => EF.Property<DateTime>(e, "CreatedAt"))
                .Skip(skip)
                .Take(take)
                .ToListAsync();

        public Task CreateAsync(T entity)
        {
            _dbSet.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<T> AsQueryable() => _dbSet.AsQueryable();
    }
}