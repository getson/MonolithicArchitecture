using System;
using BinaryOrigin.SeedWork.Commands;
using App.Application.Commands.Administrations;
using App.Core.Domain.Administrations;
using System.Threading.Tasks;
using BinaryOrigin.SeedWork.Core.Domain;
using App.Core;
using BinaryOrigin.SeedWork.Core.Extensions;

namespace App.Application.CommandHandlers.Administrations
{
    public sealed class UpdateProjectHandler : ICommandHandler<UpdateProject, bool>
    {
        private readonly IProjectRepository _projectRepository;

        public UpdateProjectHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Result<bool>> HandleAsync(UpdateProject command)
        {
            var projectOrNothing = await _projectRepository.GetByIdAsync(command.Id);
            if (projectOrNothing.HasNoValue)
            {
                return Result.Fail<bool>(ErrorMessages.ProjectNotFound);
            }
            var result = projectOrNothing.Value.UpdateDetails(command.Name, command.Description);
            if (result.IsFailure)
            {
                return Result.Fail<bool>(result.Error);
            }
            return Result.Ok(true);
        }

        public bool Validate(UpdateProject command)
        {
            return Guid.Empty!= command.Id && !command.Name.IsNullOrEmpty();
        }
    }
}