using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppFurniture.DAL.Entities;

namespace WebAppFurniture.DAL.Interfaces
{
    public interface IWarehouseProductRepository : IRepository<WarehouseProduct>
    {
        public void Detach<TEntity>(TEntity entity) where TEntity : class;
    }
}
