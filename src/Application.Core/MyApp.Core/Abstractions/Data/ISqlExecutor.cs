namespace MyApp.Core.Abstractions.Data
{
    public interface ISqlExecutor
    {
        void ExecuteSqlFile(string path);
    }
}
