using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MyApp.Core.Domain.ActivityLog
{
    public interface IActivityLogTypeRepository:IRepository<ActivityLogType>
    {
        IList<ActivityLogType> GetAll();
    }
}
