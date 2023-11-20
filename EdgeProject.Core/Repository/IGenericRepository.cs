using EdgeProject.Core.Entities;
using EdgeProject.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Core.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
         Task<IReadOnlyList<T>> GetAllAsync();

         Task<T> GetByIdAsync(int id);

         Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

         Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);

        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);

        Task Add(T entity);

        void Update(T entity);
        void Delete(T entity);

    }
}
