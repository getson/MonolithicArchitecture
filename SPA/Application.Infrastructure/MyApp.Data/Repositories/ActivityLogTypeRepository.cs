using System.Collections.Generic;
using System.Linq;
using MyApp.Core.Domain.ActivityLog;

namespace MyApp.Infrastructure.Data.Repositories
{
    public class ActivityLogTypeRepository : EfRepository<ActivityLogType>, IActivityLogTypeRepository
    {
        public ActivityLogTypeRepository(IDbContext context) : base(context)
        {
        }

        public IList<ActivityLogType> GetAll()
        {
            var query = from alt in Table
                        orderby alt.Name
                        select alt;
            return query.ToList();
        }
    }
}
