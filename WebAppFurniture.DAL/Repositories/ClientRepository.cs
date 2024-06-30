using Microsoft.AspNetCore.Identity;
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
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
     .Include(t => t.Reviews)
     .Include(t => t.Orders)
     .Include(t => t.Notifications)
     .Include(t => t.User)
     .Select(t => new Client
     {
         Id = t.Id,
         Picture = t.Picture,
         Surname = t.Surname,
         Phone = t.Phone,
         Address = t.Address,
         Reviews = t.Reviews.ToList() ?? new List<Review>(),
         Orders = t.Orders.ToList() ?? new List<Order>(),
         User = t.User ?? new IdentityUser()
     })
     .ToListAsync();
        }
         
        public async override Task<Client> GetByIdAsync(int id) => await _dbSet.AsNoTracking()
            .Include(t => t.Reviews)
            .Include(t => t.Orders)
            .Include(t => t.Notifications)
            .Include(t => t.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
