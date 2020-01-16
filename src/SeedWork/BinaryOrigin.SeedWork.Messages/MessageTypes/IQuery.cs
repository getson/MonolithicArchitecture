namespace BinaryOrigin.SeedWork.Messages
{
    /// <summary>
    /// Kind of message that will tell the system to do a query. It has only one handler
    /// </summary>
    /// <typeparam name="TQueryResult"></typeparam>
    public interface IQuery<TQueryResult>
    {
    }
}