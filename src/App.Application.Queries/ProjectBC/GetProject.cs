using System;
using BinaryOrigin.SeedWork.Queries;

namespace App.Application.Queries.ProjectBC
{
    public sealed class GetProject : IQuery<GetProjectResult>
    {
        public Guid Id { get; set; }
    }

    public sealed class GetProjectResult
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}