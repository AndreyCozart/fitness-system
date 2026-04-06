using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FitnessSystem.Filters
{
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public AuthorizeFilter(string role = "")
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var username = context.HttpContext.Session.GetString("Username");

            // Если пользователь не авторизован
            if (string.IsNullOrEmpty(username))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Если требуется определенная роль
            if (!string.IsNullOrEmpty(_role))
            {
                var userRole = context.HttpContext.Session.GetString("UserRole");
                if (userRole != _role && userRole != "Admin") // Admin имеет доступ ко всему
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                    return;
                }
            }
        }
    }
}