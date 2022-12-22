using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Server.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Server.Data.Repositories;
public sealed class BaseRepository : IBaseRepository
{
    private readonly ApplicationDbContext _context;
    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    #region Read

    public async Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : BaseEntity 
            => await _context.Set<TEntity>().AnyAsync(predicate);
    
    public async Task<TEntity> Find<TEntity>(long id) 
        where TEntity : BaseEntity 
            => await _context.Set<TEntity>().FindAsync(id);

    public async Task<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) 
        where TEntity : BaseEntity
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public async Task<List<ApplicationUser>> GetAllUsers()
    {
        return await _context.Users
            .Include(x => x.Detail)
            .Include(x => x.StringValues)
            .ToListAsync();
    }

    public async Task<List<TValue>> GetAllUsers<TValue>(Expression<Func<ApplicationUser, TValue>> selector, Expression<Func<ApplicationUser, bool>> predicate = null)
    {
        IQueryable<ApplicationUser> query = _context.Users.AsQueryable();

        if(predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.Select(selector).ToListAsync();
    }
    
  
    public async Task<IEnumerable<TEntity>> GetAll<TEntity>(
        Expression<Func<TEntity, bool>> predicate = null,
        Expression<Func<TEntity, object>> orderBy = null,
        bool isDescending = false,
        params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes) where TEntity : BaseEntity
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        query = query.Where(predicate);

        if (includes?.Any() == true)
        {
            foreach (var include in includes)
            {
                query = include(query);
            }
        }

        if (orderBy != null)
        {
            query = isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        }

        return await query.ToListAsync();
    }

    #endregion

    #region Insert

    public async Task<bool> AddWithCheck<TEntity>(TEntity entity) 
        where TEntity : BaseEntity
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _context.Entry(entity).State = EntityState.Detached;
            return false;
        }
    }

    public async Task<IEnumerable<TEntity>> AddRange<TEntity>(IEnumerable<TEntity> entities) 
        where TEntity : BaseEntity
    {
        try
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _context.Entry(entities).State = EntityState.Detached;
        }
        return entities;
    }


    #endregion
}

