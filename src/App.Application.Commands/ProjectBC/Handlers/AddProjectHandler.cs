using App.Core.Domain.ProjectBC;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Messages;
using System;
using System.Threading.Tasks;

namespace App.Application.Commands.ProjectBC.Handlers
{
    public sealed class AddProjectHandler : ICommandHandler<AddProject, Guid>
    {
        private readonly IProjectRepository _projectRepository;

        public AddProjectHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Result<Guid>> HandleAsync(AddProject command)
        {
            var projectResult = Project.Create(Guid.NewGuid(),
                                        command.Name,
                                        command.Description);
            if (projectResult.IsFailure)
            {
                return Result.Fail<Guid>(projectResult.Error);
            }
            await _projectRepository.CreateAsync(projectResult.Value);
            return Result.Ok(projectResult.Value.Id);
        }
    }
}