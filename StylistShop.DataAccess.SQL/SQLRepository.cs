using StylistShop.Core.Contracts;
using StylistShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylistShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext context; //Access to DB
        internal DbSet<T> dbSet; //Table
        //Constructor
        public SQLRepository(DataContext context)
        {
            this.context = context; //Assign DB
            this.dbSet = context.Set<T>(); //Assign table to work with (Product, ProductCategory)
        }

        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var objectClass = Find(Id);
            if (context.Entry(objectClass).State == EntityState.Detached)
            {
                dbSet.Attach(objectClass);
            }

            dbSet.Remove(objectClass);
        }

        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T objectClass)
        {
            dbSet.Add(objectClass);
        }

        public void Update(T ObjectClass)
        {
            dbSet.Attach(ObjectClass);
            //Below instruction indicates that when calling SaveChange method we are requesting
            //to EF to look for ObjectClass and update it in the DB
            context.Entry(ObjectClass).State = EntityState.Modified; 
        }
    }
}
