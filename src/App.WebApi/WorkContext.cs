using BinaryOrigin.SeedWork.Core;
using BinaryOrigin.SeedWork.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace App.WebApi
{
    public class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _userId;
        private string _fullName;
        private string _email;
        private string _username;

        public WorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                if (string.IsNullOrEmpty(_userId))
                {
                    _userId = GetClaimValue("user_id");
                }
                return _userId;
            }
        }

        private string GetClaimValue(string claimType)
        {
            var claim = _httpContextAccessor.HttpContext
                          .User.Claims
                          .FirstOrDefault(x => x.Type == claimType);
            return claim.Value.ToString();
        }

        public string FullName
        {
            get
            {
                if (_fullName.IsNullOrEmpty())
                {
                    _fullName = GetClaimValue("full_name");
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
                    _email = GetClaimValue("email");
                }

                return _email;
            }
        }

        public string UserName
        {
            get
            {
                if (_username.IsNullOrEmpty())
                {
                    _username = GetClaimValue("username");
                }

                return _username;
            }
        }
    }
}