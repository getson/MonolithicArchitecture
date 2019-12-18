using System;
using BinaryOrigin.SeedWork.Core.Domain;

namespace App.Core
{
    public sealed class UserContext : BaseEntity
    {
        public UserContext(Guid tenantId, int userId, string fullName, int subProjectId, string subProjectName, int projectId, string projectName, string email, string phone)
        {
            TenantId = tenantId;
            UserId = userId;
            FullName = fullName;
            SubProjectId = subProjectId;
            SubProjectName = subProjectName;
            ProjectId = projectId;
            ProjectName = projectName;
            Email = email;
            Phone = phone;
        }

        public Guid TenantId { get; }

        public int UserId { get; }

        public string FullName { get; }

        public int SubProjectId { get; }

        public string SubProjectName { get; }

        public int ProjectId { get; }

        public string ProjectName { get; }

        public string Email { get; }

        public string Phone { get; }
    }
}