namespace BinaryOrigin.SeedWork.Core
{
    public interface IWorkContext
    {
        string UserId { get; }
        string UserName { get; }
        string FullName { get; }
        string Email { get; }
    }
}