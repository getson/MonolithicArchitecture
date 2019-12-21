using BinaryOrigin.SeedWork.Messages;
using System.Collections.Generic;

namespace App.Application.Queries.ProjectBC
{
    public sealed class GetProjects : IQuery<GetProjectsResult>
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
    }

    public sealed class GetProjectsResult
    {
        public int Total { get; set; }
        public IEnumerable<GetProjectResult> Items { get; set; }
    }
}