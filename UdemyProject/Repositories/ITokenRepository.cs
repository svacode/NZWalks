using Microsoft.AspNetCore.Identity;

namespace UdemyProject.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
