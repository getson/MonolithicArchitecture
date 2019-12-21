using App.Application.Commands.ProjectBC;
using App.Core;
using App.Core.Domain.ProjectBC;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Core.Extensions;
using BinaryOrigin.SeedWork.Messages;
using System;
using System.Threading.Tasks;

namespace App.Application.CommandHandlers.ProjectBC
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
            await _projectRepository.UpdateAsync(projectOrNothing.Value);
            return Result.Ok(true);
        }
    }
}