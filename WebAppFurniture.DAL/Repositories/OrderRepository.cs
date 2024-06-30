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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
            .ToListAsync();
        }

        public async override Task<Order> GetByIdAsync(int id) => await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
