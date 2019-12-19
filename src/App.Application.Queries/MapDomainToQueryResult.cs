﻿using App.Application.Queries.ProjectBC;
using App.Core.Domain.ProjectBC;
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
