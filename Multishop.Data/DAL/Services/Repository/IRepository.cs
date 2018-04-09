using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Data.DAL.Services.Repository
{
    interface IRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetEntities();
        T GetDetails(int entityId);
        void Insert(T entity);
        void Delete(int entityId);
        void Update(T entity);
        void Save();
    }
}
