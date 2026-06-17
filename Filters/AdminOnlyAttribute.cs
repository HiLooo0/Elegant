using System;
using System.Web.Mvc;

namespace Elegant.Filters
{
    /// <summary>
    /// Дозволяє доступ лише автентифікованому користувачу з email elegant@gmail.com.
    /// Усі інші зареєстровані користувачі (навіть авторизовані) отримають 403 / редірект на AccessDenied.
    /// </summary>
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        private const string AdminEmail = "elegant@gmail.com";

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            return string.Equals(httpContext.User.Identity.Name, AdminEmail, StringComparison.OrdinalIgnoreCase);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // Користувач увійшов, але не адмін -> AccessDenied
                filterContext.Result = new RedirectResult("~/Account/AccessDenied");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
