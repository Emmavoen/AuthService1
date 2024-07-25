using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Contract.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllByColumnAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task Update(T entity);
        Task Delete(int id);

        Task<T> GetByColumnAsync(Expression<Func<T, bool>> predicate);
       


    }
}
