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
    public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .Include(t => t.WarehouseProducts)
                .ToListAsync();
        }

        public override async Task<Warehouse> GetByIdAsync(int id) => await _dbSet.AsNoTracking()
             .Include(t => t.WarehouseProducts)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
