using BinaryOrigin.SeedWork.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Events.Handlers
{
    class DummyProjectCreatedHandler : IEventHandler<ProjectCreated>
    {
        public Task HandleAsync(ProjectCreated message)
        {
            Console.WriteLine($"DummyHandler: {message.Id} - {message.Name}");
            return Task.CompletedTask;
        }
    }
    class Dummy2ProjectCreatedHandler : IEventHandler<ProjectCreated>
    {
        public Task HandleAsync(ProjectCreated message)
        {
            Console.WriteLine($"DummyHandler2: {message.Id} - {message.Name}");
            return Task.CompletedTask;
        }
    }
}
