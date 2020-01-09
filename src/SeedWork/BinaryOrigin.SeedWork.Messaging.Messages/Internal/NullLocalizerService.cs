namespace BinaryOrigin.SeedWork.Messages.Decorators
{
    public class NullLocalizerService : ILocalizerService
    {
        public string Localize(string message)
        {
            return message;
        }
    }
}
