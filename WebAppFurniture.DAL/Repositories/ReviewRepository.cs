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
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
            .ToListAsync();
        }

        public async override Task<Review> GetByIdAsync(int id) => await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
