using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T itemToUpdate = items.Find(i => i.Id == t.Id);

            if (itemToUpdate != null)
            {
                itemToUpdate = t;
            }
            else
            {
                throw new Exception(className + " not found!");
            }
        }

        public T Find(string Id)
        {
            T item = items.Find(i => i.Id == Id);

            if (item != null)
            {
                return item;
            }
            else
            {
                throw new Exception(className + " not found!");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T item = items.Find(i => i.Id == Id);

            if (item != null)
            {
                items.Remove(item);
            }
            else
            {
                throw new Exception(className + " not found!");
            }
        }
    }
}
