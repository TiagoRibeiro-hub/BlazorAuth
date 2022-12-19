using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Server.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Server.Data.Repositories;
public sealed class BaseRepository : IBaseRepository
{
    private readonly ApplicationDbContext _context;
    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : BaseEntity
    {
        try
        {
           return await _context.Set<TEntity>().AnyAsync(predicate);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) 
        where TEntity : BaseEntity
    {
        try
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        catch (Exception)
        {
          throw;
        }
    }

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


}

