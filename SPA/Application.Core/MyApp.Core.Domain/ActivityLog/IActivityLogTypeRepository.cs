using System.Collections.Generic;

namespace MyApp.Core.Domain.ActivityLog
{
    public interface IActivityLogTypeRepository:IRepository<ActivityLogType>
    {
        IList<ActivityLogType> GetAll();
    }
}
