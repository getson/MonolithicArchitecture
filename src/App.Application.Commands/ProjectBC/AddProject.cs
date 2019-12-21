using BinaryOrigin.SeedWork.Messages;
using System;

namespace App.Application.Commands.ProjectBC
{
    public sealed class AddProject : ICommand<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}