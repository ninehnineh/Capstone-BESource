using Parking.FindingSlotManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public Task<int> CountAll(System.Linq.Expressions.Expression<Func<T, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllItemWithCondition(System.Linq.Expressions.Expression<Func<T, bool>> expression = null, List<System.Linq.Expressions.Expression<Func<T, object>>> includes = null, System.Linq.Expressions.Expression<Func<T, int>> orderBy = null, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllItemWithConditionByNoInclude(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllItemWithPagination(System.Linq.Expressions.Expression<Func<T, bool>> expression = null, List<System.Linq.Expressions.Expression<Func<T, object>>> includes = null, System.Linq.Expressions.Expression<Func<T, int>> orderBy = null, bool disableTracking = true, int? page = null, int? pageSize = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetItemWithCondition(System.Linq.Expressions.Expression<Func<T, bool>> expression = null, List<System.Linq.Expressions.Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public Task Insert(T obj)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public Task Update(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
