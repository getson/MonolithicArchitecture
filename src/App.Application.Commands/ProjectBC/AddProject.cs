using System;
using BinaryOrigin.SeedWork.Commands;

namespace App.Application.Commands.ProjectBC
{
    public sealed class AddProject : ICommand<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}