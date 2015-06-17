using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Fzrain.EntityFramework.Repositories
{
    public abstract class FzrainRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<FzrainDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected FzrainRepositoryBase(IDbContextProvider<FzrainDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class FzrainRepositoryBase<TEntity> : FzrainRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected FzrainRepositoryBase(IDbContextProvider<FzrainDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
