using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using MyApp.Core.Interfaces.Mapping;

namespace MyApp.Infrastructure.Common.Adapter
{
    public class AutomapperTypeAdapterFactory : ITypeAdapterFactory
    {
        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

    }
}
