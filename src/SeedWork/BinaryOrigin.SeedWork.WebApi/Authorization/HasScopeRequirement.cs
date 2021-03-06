﻿using Microsoft.AspNetCore.Authorization;
using System;

namespace BinaryOrigin.SeedWork.WebApi.Authorization
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Scope { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        public HasScopeRequirement(string scope)
        {
           Scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }
    }
}
