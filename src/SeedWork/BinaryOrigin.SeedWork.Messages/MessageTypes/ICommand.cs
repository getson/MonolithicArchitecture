namespace BinaryOrigin.SeedWork.Messages
{
    /// <summary>
    /// Kind of messages that perform an action or side effects in the system,
    /// it has only one handler
    /// </summary>
    /// <typeparam name="TCommandResult"></typeparam>
    public interface ICommand<TCommandResult>
    {
    }
}