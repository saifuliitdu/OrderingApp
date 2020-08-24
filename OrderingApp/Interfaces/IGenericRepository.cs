using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrderingApp.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Find(object id);
        Task<bool> Update(T model);
        Task<T> Create(T model);
        //bool Delete(T model);
        Task<bool> Delete(object id);
        Task<IEnumerable<T>> FindAll();
        //T Get(string id);
        //IEnumerable<T> Query();
        //IEnumerable<T> Query(Expression<Func<T, bool>> filter);
    }
}
