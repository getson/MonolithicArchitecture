using System;

namespace BinaryOrigin.SeedWork.Core
{
    public interface IWorkContext
    {
        Guid UserId { get; }
        Guid TenantId { get;  }
        string UserName { get; }
        string FullName { get; }
        string Email { get; }
    }
}