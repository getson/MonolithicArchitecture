using System.Collections.Generic;
using MyApp.Core.SharedKernel.Domain;

namespace MyApp.Domain.Logging
{
    public interface ILogRepository:IRepository<Log>
    {
        void ClearLog();
        IList<Log> GetLogByIds(int[] logIds);
    }
}
