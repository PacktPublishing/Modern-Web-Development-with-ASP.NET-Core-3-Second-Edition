using Microsoft.AspNetCore.Authorization;
using System;

namespace chapter07
{
    public sealed class DayOfWeekRequirement : IAuthorizationRequirement
    {
        public const string Name = "DayOfWeek";

        public DayOfWeekRequirement(DayOfWeek dow)
        {
            this.DayOfWeek = dow;
        }

        public DayOfWeek DayOfWeek { get; }
    }
}