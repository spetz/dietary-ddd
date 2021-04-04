using System.Security.Claims;
using System.Security.Principal;

namespace Dietary.Models
{
    public interface IAuthenticationFacade
    {
        ClaimsPrincipal GetAuthentication();
    }

    public class AuthenticationFacade : IAuthenticationFacade
    {
        private readonly string _username;

        public AuthenticationFacade(string username = "test")
        {
            _username = username;
        }

        public ClaimsPrincipal GetAuthentication() => new(new GenericIdentity(_username));
    }
}