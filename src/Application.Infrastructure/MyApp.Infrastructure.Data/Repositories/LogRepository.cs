using System.Collections.Generic;
using System.Linq;
using MyApp.Domain.Logging;
using MyApp.Infrastructure.Data.Extensions;

namespace MyApp.Infrastructure.Data.Repositories
{
    public class LogRepository : EfRepository<Log>, ILogRepository
    {
        public LogRepository(IDbContext context) : base(context)
        {
        }

        public void ClearLog()
        {
            //do all databases support "Truncate command"?
            var logTableName = EfDbContext.GetTableName<Log>();
            EfDbContext.ExecuteSqlCommand($"TRUNCATE TABLE [{logTableName}]");
        }
        public virtual IList<Log> GetLogByIds(int[] logIds)
        {
            if (logIds == null || logIds.Length == 0)
                return new List<Log>();

            var query = from l in Table
                        where logIds.Contains(l.Id)
                        select l;
            var logItems = query.ToList();

            //sort by passed identifiers
            return logIds.Select(id => logItems.Find(x => x.Id == id)).Where(log => log != null).ToList();
        }
    }
}
