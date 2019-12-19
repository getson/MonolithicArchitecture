namespace BinaryOrigin.SeedWork.Core.Domain
{
    public interface IResult
    {
        bool IsFailure { get; }
        bool IsSuccess { get; }
    }
}