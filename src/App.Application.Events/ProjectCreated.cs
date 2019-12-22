using BinaryOrigin.SeedWork.Messages;
using System;

namespace App.Application.Events
{
    public class ProjectCreated : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}