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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await _dbSet
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var product in products)
                {
                    // Проверка данных
                    Console.WriteLine($"Product ID: {product.Id}, Name: {product.Name}");
                }

                return await _dbSet
                    .AsNoTracking()
                    .Include(p => p.Orders)
                    .Include(p => p.WarehouseProducts)
                    .Include(p => p.ParameterProducts)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Здесь вы можете вывести подробную информацию об ошибке
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                Console.WriteLine($"Трассировка стека: {ex.StackTrace}");
                throw; // или возвращаете пустой список, если это приемлемо
            }
        }

        public async override Task<Product> GetByIdAsync(int id) => await _dbSet.AsNoTracking()
            .Include(t => t.Orders)
            .Include(t => t.WarehouseProducts)
            .Include(t => t.ParameterProducts)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
