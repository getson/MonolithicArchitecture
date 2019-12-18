using App.Application.Queries.Administrations;
using App.Core.Domain.Administrations;
using AutoMapper;
using BinaryOrigin.SeedWork.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Queries
{
    public class MapDomainToQueryResult : Profile, IMapperProfile
    {
        public int Order => 1;
        public MapDomainToQueryResult()
        {
            CreateMap<Project, GetProjectResult>();
        }
    }
}
