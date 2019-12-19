using System;
using BinaryOrigin.SeedWork.Core.Domain;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    public interface IData : IProjectable
    {
        Guid Id { get; set; }
    }
}