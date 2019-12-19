using System;
using BinaryOrigin.SeedWork.Core.Domain;
using BinaryOrigin.SeedWork.Core.Extensions;

namespace App.Core.Domain.ProjectBC
{
    public sealed class Project : BaseEntity
    {
        public string name { get; private set; }
        public string Description { get; private set; }
        public static Result<Project> Create(Guid id, string name, string description)
        {
            if (id == Guid.Empty)
            {
                return Result.Fail<Project>(ErrorMessages.InvalidId);
            }
            if (name.IsNullOrEmpty())
            {
                return Result.Fail<Project>(ErrorMessages.NameShouldNotBeEmpty);
            }
            var project = new Project
            {
                Id = id,
                name = name,
                Description = description
            };
            return Result.Ok(project);
        }

        public Result UpdateDetails(string name, string description)
        {
            if (name.IsNullOrEmpty())
            {
                return Result.Fail(ErrorMessages.NameShouldNotBeEmpty);
            }
            this.name = name;
            Description = description;

            return Result.Ok();
        }
    }
}