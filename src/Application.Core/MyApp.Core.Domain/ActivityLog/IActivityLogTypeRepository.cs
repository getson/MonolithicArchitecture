using System.Collections.Generic;
using MyApp.SharedKernel.Domain;

namespace MyApp.Domain.ActivityLog
{
    public interface IActivityLogTypeRepository:IRepository<ActivityLogType>
    {
        IList<ActivityLogType> GetAll();
    }
}
