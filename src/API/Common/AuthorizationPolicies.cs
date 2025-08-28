using Microsoft.AspNetCore.Authorization;

namespace API.Common
{
    public static class AuthorizationPolicies
    {
        public const string AdminPolicy = "RequireAdminRole";

        public static void AddAuthorizationPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(AdminPolicy, policy =>
                policy.RequireRole("Admin"));
        }
    }
}