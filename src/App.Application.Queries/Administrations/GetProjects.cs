using System;
using System.Collections.Generic;
using BinaryOrigin.SeedWork.Queries;

namespace App.Application.Queries.Administrations
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