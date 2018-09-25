using System.Linq;
using StylistShop.Core.Models;

namespace StylistShop.Core.Contracts
{
    //Interface created from InMemoryRepository
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Collection();
        void Commit();
        void Delete(string Id);
        T Find(string Id);
        void Insert(T objectClass);
        void Update(T ObjectClass);
    }
}