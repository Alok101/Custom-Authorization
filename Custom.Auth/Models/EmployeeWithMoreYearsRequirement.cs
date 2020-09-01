using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custom.Auth.Models
{
    public class EmployeeWithMoreYearsRequirement : IAuthorizationRequirement
    {
        public EmployeeWithMoreYearsRequirement(int years)
        {
            Years = years;
        }
        public int Years { get;}
    }
}
