using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Custom.Auth.Models
{
    public class CustomAuthenticationManager : ICustomAuthenticationManager
    {
        private readonly IList<UserCred> users = new List<UserCred>
        {
            new UserCred{Username="user1",Password="password1",Roles="Administrator"},
            new UserCred{Username="user2",Password="password2",Roles="User"}
        };
        private readonly IDictionary<string, Tuple<string,string>> tokens =new Dictionary<string, Tuple<string, string>>();
        public IDictionary<string, Tuple<string, string>> Tokens => tokens;
        public string Authenticate(string username, string password)
        {
            if(!users.Any(x=>x.Username==username && x.Password == password))
            {
                return null;
            }
            var token = Guid.NewGuid().ToString();
            tokens.Add(token,new Tuple<string, string>(username,users.First(t=>t.Username==username && t.Password==password).Roles));
            return token;
        }
    }
}
