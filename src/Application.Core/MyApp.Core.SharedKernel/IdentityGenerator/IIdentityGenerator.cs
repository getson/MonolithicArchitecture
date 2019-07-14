using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.SharedKernel.IdentityGenerator
{
    public interface IIdentityGenerator
    {
        int GenerateId(string entityType);
    }
}
