using EdgeProject.Core.Entities;
using EdgeProject.Core.Repository;
using EdgeProject.Core.Specifications;
using EdgeProject.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>) await dbContext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
;           }
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

      

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return ApplySpecification(spec).CountAsync();
        }
        private IQueryable<T> ApplySpecification (ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>(), spec);
        }

        public async Task Add(T entity)
         => await dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity)
        => dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        => dbContext.Set<T>().Remove(entity);
    }
}
