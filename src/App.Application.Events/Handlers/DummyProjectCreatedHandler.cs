using BinaryOrigin.SeedWork.Messages;
using System;
using System.Threading.Tasks;

namespace App.Application.Events.Handlers
{
    internal class DummyProjectCreatedHandler : IEventHandler<ProjectCreated>
    {
        public Task HandleAsync(ProjectCreated message)
        {
            Console.WriteLine($"DummyHandler: {message.Id} - {message.Name}");
            return Task.CompletedTask;
        }
    }

    internal class Dummy2ProjectCreatedHandler : IEventHandler<ProjectCreated>
    {
        public Task HandleAsync(ProjectCreated message)
        {
            Console.WriteLine($"DummyHandler2: {message.Id} - {message.Name}");
            return Task.CompletedTask;
        }
    }
}