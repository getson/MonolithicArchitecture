using System;
using BinaryOrigin.SeedWork.Commands;

namespace App.Application.Commands.ProjectBC
{
    public sealed class DeleteProject : ICommand<bool>
    {
        public DeleteProject()
        {
        }

        public DeleteProject(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}