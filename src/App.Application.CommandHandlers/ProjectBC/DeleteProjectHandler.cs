using App.Application.Commands.ProjectBC;
using App.Core.Domain.ProjectBC;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Messages;
using System;
using System.Threading.Tasks;

namespace App.Application.CommandHandlers.ProjectBC
{
    public sealed class DeleteProjectHandler : ICommandHandler<DeleteProject, bool>
    {
        private readonly IProjectRepository _projectRepository;

        public DeleteProjectHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IProjectRepository ProjectRepository { get; }

        public async Task<Result<bool>> HandleAsync(DeleteProject command)
        {
            var projectOrNothing = await _projectRepository.GetByIdAsync(command.Id);
            if (projectOrNothing.HasNoValue)
            {
                return Result.Ok(true);
            }
            await _projectRepository.DeleteAsync(projectOrNothing.Value);
            return Result.Ok(true);
        }
    }
}