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
    public class ProductGroupRepository : Repository<ProductGroup>, IProductGroupRepository
    {
        public ProductGroupRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ProductGroup>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .Include(t => t.Products)
                .ToListAsync();
        }

        public async override Task<ProductGroup> GetByIdAsync(int id) => await _dbSet.AsNoTracking()
            .Include(t => t.Products)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
