using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json.Linq;
using Server.Entities.Entities;
using System.Linq.Expressions;

namespace Server.Data.Repositories;

public interface IBaseRepository
{
    #region Read

    Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;

    Task<TEntity> Find<TEntity>(long id) where TEntity : BaseEntity;

    Task<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity;

    Task<List<ApplicationUser>> GetAllUsers();

    Task<List<TValue>> GetAllUsers<TValue>(Expression<Func<ApplicationUser, TValue>> selector, Expression<Func<ApplicationUser, bool>> predicate = null);

    Task<IEnumerable<TEntity>> GetAll<TEntity>(
        Expression<Func<TEntity, bool>> predicate = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool isDescending = false,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes) where TEntity : BaseEntity;


    #endregion

    #region Insert

    Task<bool> AddWithCheck<TEntity>(TEntity entity) where TEntity : BaseEntity;

    Task<IEnumerable<TEntity>> AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

    #endregion
}

