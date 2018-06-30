using System.Collections.Generic;
using MyApp.Core.SharedKernel;

namespace MyApp.Core.Domain.ActivityLog
{
    public interface IActivityLogTypeRepository:IRepository<ActivityLogType>
    {
        IList<ActivityLogType> GetAll();
    }
}
