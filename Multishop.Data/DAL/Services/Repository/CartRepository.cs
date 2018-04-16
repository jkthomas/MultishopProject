using Multishop.Data.DAL.Context;
using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Data.DAL.Services.Repository
{
    public class CartRepository : IManagementRepository<Cart>
    {
        private ApplicationDbContext _dbContext;

        public CartRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public void Delete(string cartId)
        {
            Cart cart = _dbContext.Carts.Find(cartId);
            _dbContext.Carts.Remove(cart);
        }

        public Cart GetDetails(string cartId)
        {
            return _dbContext.Carts.Find(cartId);
        }

        public IEnumerable<Cart> GetEntities()
        {
            return _dbContext.Carts.ToList();
        }

        public void Insert(Cart cart)
        {
            _dbContext.Carts.Add(cart);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Cart cart)
        {
            _dbContext.Entry(cart).State = System.Data.Entity.EntityState.Modified;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

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
