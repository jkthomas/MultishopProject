using Multishop.Data.DAL.Context;
using Multishop.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Data.DAL.Services.Repository
{
    public class InventoryRepository : IRepository<Inventory>
    {
        private ApplicationDbContext _dbContext;

        public InventoryRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public void Delete(int inventoryId)
        {
            Inventory inventory = _dbContext.Inventories.Find(inventoryId);
            _dbContext.Inventories.Remove(inventory);
        }

        public Inventory GetDetails(int? inventoryId)
        {
            return _dbContext.Inventories.Find(inventoryId);
        }

        public IEnumerable<Inventory> GetEntities()
        {
            return _dbContext.Inventories.ToList();
        }

        public void Insert(Inventory inventory)
        {
            _dbContext.Inventories.Add(inventory);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Inventory inventory)
        {
            _dbContext.Entry(inventory).State = System.Data.Entity.EntityState.Modified;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

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
