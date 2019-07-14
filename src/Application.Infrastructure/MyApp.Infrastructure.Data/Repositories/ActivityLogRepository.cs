using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MyApp.Core.SharedKernel.Specification;
using MyApp.Domain.ActivityLog;

namespace MyApp.Infrastructure.Data.Repositories
{
    public class ActivityLogRepository : EfRepository<ActivityLog>, IActivityLogRepository
    {
        public ActivityLogRepository(IDbContext context) : base(context)
        {
        }

        public ActivityLogType GetById(object id)
        {
            throw new NotImplementedException();
        }

        public void Insert(ActivityLogType entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<ActivityLogType> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(ActivityLogType entity)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<ActivityLogType> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(ActivityLogType entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<ActivityLogType> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ActivityLogType> AllMatching(ISpecification<ActivityLogType> specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ActivityLogType> GetFiltered(Expression<Func<ActivityLogType, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ActivityLogType> GetFilteredNoTracking(Expression<Func<ActivityLogType, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
