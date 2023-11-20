using EdgeProject.Core;
using EdgeProject.Core.Entities;
using EdgeProject.Core.Repository;
using EdgeProject.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext context;

        private Hashtable _repositories;

        public UnitOfWork(StoreContext context)
        {
            this.context = context;
            _repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(context);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
        => await context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        => await context.DisposeAsync();

        
    }
}
