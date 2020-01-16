namespace BinaryOrigin.SeedWork.Messages
{
    public interface ICommandHandlerDecorator<TCommand, TCommandResult>
        : ICommandHandler<TCommand, TCommandResult>
            where TCommand : ICommand<TCommandResult>
    {
        ICommandHandler<TCommand, TCommandResult> Handler { get; }
    }
}