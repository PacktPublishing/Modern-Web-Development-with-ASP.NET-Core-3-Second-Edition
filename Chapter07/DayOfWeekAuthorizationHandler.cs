using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter07
{
    public sealed class DayOfWeekAuthorizationHandler : AuthorizationHandler<DayOfWeekRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            DayOfWeekRequirement requirement)
        {
            if ((context.Resource is DayOfWeek requestedRequirement) && (requestedRequirement == requirement.DayOfWeek))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
