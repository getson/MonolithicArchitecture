using BinaryOrigin.SeedWork.Core.Domain;
using System;

namespace BinaryOrigin.SeedWork.Persistence.Ef
{
    public interface IData : IProjectable
    {
        Guid Id { get; set; }
    }
}