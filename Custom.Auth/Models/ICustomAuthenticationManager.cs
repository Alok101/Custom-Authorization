using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custom.Auth.Models
{
    public interface ICustomAuthenticationManager
    {
        string Authenticate(string username, string password);
        public IDictionary<string, Tuple<string,string>> Tokens { get; }
    }
}
