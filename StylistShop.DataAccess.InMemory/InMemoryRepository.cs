using StylistShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace StylistShop.DataAccess.InMemory
{
    //<T> is a placeholder as we will declare the class as a generic class
    public class InMemoryRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            //We do some reflection here and we pass the class as a reference
            //GET THE ACTUAL NAME OF A CLASS
            className = typeof(T).Name;
            //Check whether the class in the cache, otherwise add it
            items = cache[className] as List<T>;
            if (items==null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T objectClass)
        {
            items.Add(objectClass);
        }

        public void Update(T ObjectClass)
        {
            T objectToUpdate = items.Find(i => i.Id == ObjectClass.Id);
            if (objectToUpdate != null)
            {
                objectToUpdate = ObjectClass;
            }
            else
            {
                throw new Exception(className + "not found");
            }
        }

        public T Find(string Id)
        {
            T objectClass = items.Find(i => i.Id == Id);
            if (objectClass != null)
            {
                return objectClass;
            }
            else
            {
                throw new Exception(className + "not found");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T objectToDelete = items.Find(i => i.Id == Id);
            if (objectToDelete != null)
            {
                items.Remove(objectToDelete);
            }
            else
            {
                throw new Exception(className + "not found");
            }
        }
    }
}
