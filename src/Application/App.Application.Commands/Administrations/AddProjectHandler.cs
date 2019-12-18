using App.Application.Commands.Administrations;
using BinaryOrigin.SeedWork.Commands;
using App.Core.Domain.Administrations;
using BinaryOrigin.SeedWork.Core.Extensions;
using System;
using System.Threading.Tasks;
using BinaryOrigin.SeedWork.Core.Domain;

namespace App.Application.CommandHandlers.Administrations
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

        public bool Validate(AddProject command)
        {
            return !command.Name.IsNullOrEmpty();
        }
    }
}