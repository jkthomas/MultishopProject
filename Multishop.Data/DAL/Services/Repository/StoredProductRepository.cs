using Multishop.Data.DAL.Context;
using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Data.DAL.Services.Repository
{
    public class StoredProductRepository : IProductionRepository<StoredProduct>
    {
        private ApplicationDbContext _dbContext;

        public StoredProductRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Delete(int storedProductId)
        {
            StoredProduct storedProduct = _dbContext.StoredProducts.Find(storedProductId);
            _dbContext.StoredProducts.Remove(storedProduct);
        }

        public StoredProduct GetDetails(int? storedProductId)
        {
            return _dbContext.StoredProducts.Find(storedProductId);
        }

        public IEnumerable<StoredProduct> GetEntities()
        {
            return _dbContext.StoredProducts.ToList();
        }

        public void Insert(StoredProduct storedProduct)
        {
            _dbContext.StoredProducts.Add(storedProduct);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(StoredProduct storedProduct)
        {
            _dbContext.Entry(storedProduct).State = System.Data.Entity.EntityState.Modified;
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
