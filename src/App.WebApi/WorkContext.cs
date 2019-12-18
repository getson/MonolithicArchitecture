using System;
using System.Linq;
using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace App.WebApi
{
    public class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private Guid _userId;
        private string _fullName;
        private string _email;

        public WorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid TenantId => throw new NotImplementedException();

        public Guid UserId
        {
            get
            {
                if (_userId.ToString().IsNullOrEmpty())
                {
                    Guid.TryParse(_httpContextAccessor.HttpContext
                        .User
                        .Claims
                        .FirstOrDefault(c => c.Type == "user_id")
                        ?.Value, out _userId);
                }
                return _userId;
            }
        }

        public string FullName
        {
            get
            {
                if (_fullName.IsNullOrEmpty())
                {
                    _fullName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                }

                return _fullName;
            }
        }

        public string Email
        {
            get
            {
                if (_email.IsNullOrEmpty())
                {
                    _email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                }

                return _email;
            }
        }

        public string Phone => throw new NotImplementedException();

        public string UserName => throw new NotImplementedException();
    }
}