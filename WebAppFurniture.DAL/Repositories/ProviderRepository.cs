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
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Provider>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                 .Include(t => t.Products)
                 .ToListAsync();
        }

        public override async Task<Provider> GetByIdAsync(int id) => await _dbSet.AsNoTracking()
            .Include(t => t.Products)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
