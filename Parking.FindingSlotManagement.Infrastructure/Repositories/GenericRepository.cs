using Microsoft.EntityFrameworkCore;
using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using Parking.FindingSlotManagement.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ParkZDbContext _dbContext;

        public GenericRepository(ParkZDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> CountAll(Expression<Func<T, bool>> expression = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (expression != null) query = query.Where(expression);
            return await query.CountAsync();
        }

        public async Task Delete(T obj)
        {
            _dbContext.Set<T>().Remove(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllItemWithCondition(Expression<Func<T, bool>> expression = null, List<Expression<Func<T, object>>> includes = null, Expression<Func<T, int>> orderBy = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            if (expression != null) query = query.Where(expression);
            if (orderBy != null)
                return await query.OrderByDescending(orderBy).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllItemWithConditionByNoInclude(Expression<Func<T, bool>> expression)
        {
            var lst = await _dbContext.Set<T>().Where(expression).ToListAsync();
            if (lst.Count <= 0)
            {
                return null;
            }
            return lst;
        }

        public async Task<IEnumerable<T>> GetAllItemWithPagination(Expression<Func<T, bool>> expression = null, List<Expression<Func<T, object>>> includes = null, Expression<Func<T, int>> orderBy = null, bool disableTracking = true, int? page = null, int? pageSize = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            if (expression != null) query = query.Where(expression);
            if (orderBy != null)
                return await query.OrderByDescending(orderBy).Skip(((int)page - 1) * (int)pageSize)
                        .Take((int)pageSize).ToListAsync();
            return await query.Skip(((int)page - 1) * (int)pageSize)
                        .Take((int)pageSize).ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            var rs = await _dbContext.Set<T>().FindAsync(id);
            if (rs == null)
            {
                return null;
            }
            return rs;
        }

        public async Task<T> GetItemWithCondition(Expression<Func<T, bool>> expression = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
            if (expression != null) query = query.Where(expression);
            return await query.FirstOrDefaultAsync();
        }

        public async Task Insert(T obj)
        {
            _dbContext.Set<T>().Add(obj);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(T obj)
        {
            _dbContext.ChangeTracker.Clear();
            _dbContext.Set<T>().Update(obj);
            await _dbContext.SaveChangesAsync();
        }
    }
}
