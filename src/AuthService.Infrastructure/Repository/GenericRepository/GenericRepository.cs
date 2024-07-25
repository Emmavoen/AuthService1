using AuthService.Domain.DTOs.Request;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Contract.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
//using System.Data.Entity;

//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repository.GenericRepository
{
    //no nuget package registration in startup.cs
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext appDbContext;
        protected DbSet<T> dbSet;
        public GenericRepository(AppDbContext _appDbContext)
        {
            appDbContext = _appDbContext;
            dbSet =  appDbContext.Set<T>();   
        }
        public async Task<T> Add(T entity)
        {
            var result = await dbSet.AddAsync(entity);
            // await appDbContext.SaveChangesAsync();  // Save the changes to the database
            return result.Entity;
           // return entity;

            /*var result = await dbSet.AddAsync(entity);
              return result;*/

        }

        public async Task Delete(int id)
        {
            T existing = await dbSet.FindAsync(id);
            dbSet.Remove(existing);
            //await appDbContext.SaveChangesAsync();

        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var result = await dbSet.ToListAsync();
           // await appDbContext.SaveChangesAsync();
            return result;
        }

        public async Task<T> GetById(int id)
        {
            var result = await dbSet.FindAsync(id);
            //await appDbContext.SaveChangesAsync(); //not needed any more since unit of work does the saving
            return result;

        }

        public async Task<IEnumerable<T>> GetAllByColumnAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> GetByColumnAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }


        public async Task Update(T entity)
        {
            //dbSet.Attach(entity);
             appDbContext.Entry(entity).State = EntityState.Modified;
            // appDbContext.SaveChanges();
            //throw new NotImplementedException();
        }

    }
}
