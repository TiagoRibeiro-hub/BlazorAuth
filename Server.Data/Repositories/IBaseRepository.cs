using Server.Entities.Entities;
using System.Linq.Expressions;

namespace Server.Data.Repositories;

public interface IBaseRepository
{
    // READ
    Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;
    Task<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;

    // INSERT
    Task<bool> AddWithCheck<TEntity>(TEntity entity) where TEntity : BaseEntity;
}

