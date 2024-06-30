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
    public class ParameterRepository : Repository<Parameter>, IParameterRepository
    {
        public ParameterRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Parameter>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                .Include((t => t.ParameterProducts))
            .ToListAsync();
        }

        public override async Task<Parameter> GetByIdAsync(int id) => await _dbSet.AsNoTracking()
            .Include((t => t.ParameterProducts))
            .FirstOrDefaultAsync(a => a.Id == id);

    }
}
