namespace BinaryOrigin.SeedWork.Messages
{
    public interface ILocalizerService
    {
        string Localize(string message);
    }
    public class NullLocalizerService : ILocalizerService
    {
        public string Localize(string message)
        {
            return message;
        }
    }
}
