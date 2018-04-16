using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Data.DAL.Services.Repository
{
    public interface IProductionRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetEntities();
        T GetDetails(int? entityId);
        void Insert(T entity);
        void Delete(int entityId);
        void Update(T entity);
        void Save();
    }

    public interface IManagementRepository<T> : IDisposable where T : class
    {
        IEnumerable<T> GetEntities();
        T GetDetails(string entityId);
        void Insert(T entity);
        void Delete(string entityId);
        void Update(T entity);
        void Save();
    }

}
