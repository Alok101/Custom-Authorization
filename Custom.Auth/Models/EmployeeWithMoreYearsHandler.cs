using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Custom.Auth.Models
{
    public class EmployeeWithMoreYearsHandler : AuthorizationHandler<EmployeeWithMoreYearsRequirement>
    {
        private readonly IEmployeeNumberOfYears employeeNumberOfYears;
        public EmployeeWithMoreYearsHandler(IEmployeeNumberOfYears employeeNumberOfYears)
        {
            this.employeeNumberOfYears = employeeNumberOfYears;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeeWithMoreYearsRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                return Task.CompletedTask;
            }
            var name = context.User.FindFirst(c => c.Type == ClaimTypes.Name);
            var yearofExperience = employeeNumberOfYears.Get(name.Value);
            if (yearofExperience >= requirement.Years)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
