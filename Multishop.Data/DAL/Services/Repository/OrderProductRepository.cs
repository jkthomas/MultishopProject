using Multishop.Data.DAL.Context;
using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Data.DAL.Services.Repository
{
    public class OrderProductRepository : IProductionRepository<OrderProduct>
    {
        private ApplicationDbContext _dbContext;

        public OrderProductRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Delete(int orderProductId)
        {
            OrderProduct orderProduct = _dbContext.OrderProducts.Find(orderProductId);
            _dbContext.OrderProducts.Remove(orderProduct);
        }

        public OrderProduct GetDetails(int? orderProductId)
        {
            return _dbContext.OrderProducts.Find(orderProductId);
        }

        public IEnumerable<OrderProduct> GetEntities()
        {
            return _dbContext.OrderProducts.ToList();
        }

        public void Insert(OrderProduct orderProduct)
        {
            _dbContext.OrderProducts.Add(orderProduct);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(OrderProduct orderProduct)
        {
            _dbContext.Entry(orderProduct).State = System.Data.Entity.EntityState.Modified;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
