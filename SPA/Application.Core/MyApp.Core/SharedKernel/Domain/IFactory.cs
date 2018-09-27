namespace MyApp.Core.SharedKernel.Domain
{
    public interface IFactory<out TEntity> where TEntity : BaseEntity
    {
        TEntity CreateDefault();
    }
}
