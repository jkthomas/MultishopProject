using Multishop.Data.DAL.Context;
using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Data.DAL.Services.Repository
{
    public class ProductRepository : IProductionRepository<Product>
    {
        private ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Delete(int productId)
        {
            Product product = _dbContext.Products.Find(productId);
            _dbContext.Products.Remove(product);
        }

        public Product GetDetails(int? productId)
        {
            return _dbContext.Products.Find(productId);
        }

        public IEnumerable<Product> GetEntities()
        {
            return _dbContext.Products.ToList();
        }

        public void Insert(Product product)
        {
            _dbContext.Products.Add(product);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Product product)
        {
            _dbContext.Entry(product).State = System.Data.Entity.EntityState.Modified;
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
