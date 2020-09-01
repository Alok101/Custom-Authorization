using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custom.Auth.Models
{
    public class EmployeeNumberOfYears:IEmployeeNumberOfYears
    {
        public int Get(string name)
        {
            if (name == "test1")
            {
                return 21;
            }
            return 10;
        }
    }
}
