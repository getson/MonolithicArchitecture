using System;

namespace BinaryOrigin.SeedWork.Commands
{
    public sealed class CommandResponse<TCommandResult>
    {
        public CommandResponse(TCommandResult result)
        {
            Result = result;
        }

        public CommandResponse(Exception exception)
        {
            Error = new CommandResponseError(exception);
        }

        public bool Successful => Error == null;

        public bool HasResult => Result != null;

        public TCommandResult Result { get; private set; }

        public CommandResponseError Error { get; private set; }
    }
}