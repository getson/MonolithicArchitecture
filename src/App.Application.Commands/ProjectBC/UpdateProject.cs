﻿using BinaryOrigin.SeedWork.Messages;
using System;

namespace App.Application.Commands.ProjectBC
{
    public sealed class UpdateProject : ICommand<bool>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}