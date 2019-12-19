using System;
using BinaryOrigin.SeedWork.Commands;
using App.Application.Commands.Administrations;
using App.Core.Domain.Administrations;
using System.Threading.Tasks;
using BinaryOrigin.SeedWork.Core.Domain;

namespace App.Application.CommandHandlers.Administrations
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

        public bool Validate(DeleteProject command)
        {
            return command.Id != Guid.Empty;
        }
    }
}