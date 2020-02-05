using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Messages;
using System.Collections.Generic;

namespace App.Application.Queries.ProjectBC
{
    public sealed class GetProjects : IQuery<PaginatedItemsResult<GetProjectResult>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}