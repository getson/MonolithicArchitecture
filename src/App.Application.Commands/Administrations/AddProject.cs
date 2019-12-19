using System;
using BinaryOrigin.SeedWork.Commands;

namespace App.Application.Commands.Administrations
{
    public sealed class AddProject : ICommand<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}