using System.Collections.Generic;
using MyApp.Core.SharedKernel;

namespace MyApp.Core.Domain.Logging
{
    public interface ILogRepository:IRepository<Log>
    {
        void ClearLog();
        IList<Log> GetLogByIds(int[] logIds);
    }
}
