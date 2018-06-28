using System.Collections.Generic;

namespace MyApp.Core.Domain.Logging
{
    public interface ILogRepository:IRepository<Log>
    {
        void ClearLog();
        IList<Log> GetLogByIds(int[] logIds);
    }
}
