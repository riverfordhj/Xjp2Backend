using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Models.Authentication.JWT.AuthHelper
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserService _userService;

        public PermissionHandler(IUserService userService)
        {
            _userService = userService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;

            var isAuthenticated = context.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                Guid userId;
                if (!Guid.TryParse(context.User.Claims.SingleOrDefault(s => s.Type == "id").Value, out userId))
                {
                    return Task.CompletedTask;
                }
                var functions = _userService.GetFunctionsByUserId(userId);
                //var requestUrl = httpContext.Request.Path.Value.ToLower();
                RouteEndpoint re = context.Resource as RouteEndpoint;
                if (re != null)
                {
                    var requestUrl =re.RoutePattern.RawText.ToLower();
                    if (functions != null && functions.Count > 0 && functions.Contains(requestUrl))
                    {
                        context.Succeed(requirement);
                    }
                }



            }
            return Task.CompletedTask;
        }
    }
}
