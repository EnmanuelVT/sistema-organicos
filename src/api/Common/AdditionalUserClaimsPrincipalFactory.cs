using System.Security.Claims;
using ENTIDAD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace API.Common;

public class AdditionalUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<Usuario>
{
    public AdditionalUserClaimsPrincipalFactory(
        UserManager<Usuario> userManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
    { }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var roles = await UserManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        return identity;
    }
}