using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Data;
using WebAppFurniture.DAL.Entities;
using WebAppFurniture.DAL.Interfaces;

namespace WebAppFurniture.DAL.Repositories
{
    public class WarehouseProductRepository : Repository<WarehouseProduct>, IWarehouseProductRepository
    {
        public WarehouseProductRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<WarehouseProduct>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
            .ToListAsync();
        }

        public override async Task<WarehouseProduct> GetByIdAsync(int id)=> await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        public void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = _context.Entry(entity);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}
