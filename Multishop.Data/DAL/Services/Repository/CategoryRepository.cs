using Multishop.Data.DAL.Context;
using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Data.DAL.Services.Repository
{
    public class CategoryRepository : IProductionRepository<Category>
    {
        private ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void Delete(int categoryId)
        {
            Category category = _dbContext.Categories.Find(categoryId);
            _dbContext.Categories.Remove(category);
        }

        public Category GetDetails(int? categoryId)
        {
            return _dbContext.Categories.Find(categoryId);
        }

        public IEnumerable<Category> GetEntities()
        {
            return _dbContext.Categories.ToList();
        }

        public void Insert(Category category)
        {
            _dbContext.Categories.Add(category);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(Category category)
        {
            _dbContext.Entry(category).State = System.Data.Entity.EntityState.Modified;
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
