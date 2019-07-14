namespace MyApp.SharedKernel.Domain
{
    public interface IFactory<out TEntity> where TEntity : BaseEntity
    {
        TEntity CreateDefault();
    }
}
