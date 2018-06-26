using System;
using Microsoft.AspNetCore.Http;
using MyApp.Core.Domain.User;
using MyApp.Core.Interfaces.Web;

namespace MyApp.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string CUSTOMER_COOKIE_NAME = ".MyApp.User";

        #endregion

        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserAgentHelper _userAgentHelper;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="userAgentHelper">User gent helper</param>
        public WebWorkContext(IHttpContextAccessor httpContextAccessor,
            IUserAgentHelper userAgentHelper)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userAgentHelper = userAgentHelper;


        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get MyApp customer cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetCustomerCookie()
        {
            return _httpContextAccessor.HttpContext?.Request?.Cookies[CUSTOMER_COOKIE_NAME];
        }

        /// <summary>
        /// Set MyApp customer cookie
        /// </summary>
        /// <param name="customerGuid">Guid of the customer</param>
        protected virtual void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
                return;

            //delete current cookie value
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(CUSTOMER_COOKIE_NAME);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (customerGuid == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(CUSTOMER_COOKIE_NAME, customerGuid.ToString(), options);
        }



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        public virtual User CurrentUser { get; set; }
        /// <summary>
        /// Gets or sets current user working language
        /// </summary>

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}
