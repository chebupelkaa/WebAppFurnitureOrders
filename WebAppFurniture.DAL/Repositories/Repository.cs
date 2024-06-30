using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Data;
using WebAppFurniture.DAL.Interfaces;

namespace WebAppFurniture.DAL.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        protected ApplicationContext _context;

        public Repository(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public abstract Task<T> GetByIdAsync(int id);
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public async Task CreateAsync(T item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T item)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var idProperty = item.GetType().GetProperty("Id");
                    if (idProperty == null)
                    {
                        throw new ArgumentException("Item does not have an Id property.");
                    }

                    var id = (int)idProperty.GetValue(item);
                    var existingEntity = await _dbSet.FindAsync(id);
                    if (existingEntity == null)
                    {
                        throw new ArgumentException($"{id} not found.");
                    }

                    _context.Entry(existingEntity).State = EntityState.Detached;
                    _context.Entry(item).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            //_dbSet.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
