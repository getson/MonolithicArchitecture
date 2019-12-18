﻿using System;
using BinaryOrigin.SeedWork.Commands;

namespace App.Application.Commands.Administrations
{
    public sealed class UpdateProject : ICommand<bool>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}