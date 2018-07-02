using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using MyApp.Core.SharedKernel.IdentityGenerator;

namespace MyApp.Infrastructure.Data.IdentityGenerator
{
   public class IdentityGenerator:IIdentityGenerator
    {
        
        public int GenerateId(string entityType)
        {
           return new Random().Next();
        }
    }
}
