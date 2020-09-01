using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Custom.Auth.Models
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    { }
    public class CustomAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly ICustomAuthenticationManager customAuthenticationManager;
        public CustomAuthenticationHandler(
            IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ICustomAuthenticationManager customAuthenticationManager) : base(options, logger, encoder, clock)
        {
            this.customAuthenticationManager = customAuthenticationManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("UnAuthorize");
            string authorizationHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
                return AuthenticateResult.Fail("UnAuthorize");
            if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Fail("UnAuthorize");
            string token = authorizationHeader.Substring("bearer".Length).Trim();
            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("UnAuthorize");
            try
            {
                return ValidateToken(token);
            }
            catch
            {
                return AuthenticateResult.Fail("UnAuthorize");
            }
        }
        private AuthenticateResult ValidateToken(string token)
        {
            var validateToken = customAuthenticationManager.Tokens.FirstOrDefault(x => x.Key == token);
            if (string.IsNullOrEmpty(validateToken.Key))
                return AuthenticateResult.Fail("UnAuthorize");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,validateToken.Value.Item1),
                 new Claim(ClaimTypes.Role,validateToken.Value.Item2)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new GenericPrincipal(identity, new[] { validateToken.Value.Item2 });
            var ticket = new AuthenticationTicket(principal,Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

    }
}
