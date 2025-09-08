using Microsoft.AspNetCore.Authorization;

namespace API.Common
{
    public static class AuthorizationPolicies
    {
        public const string AdminPolicy = "RequireAdminRole";
        public const string Analista = "RequireAnalistaRole";
        public const string Solicitante = "RequireSolicitanteRole";
        public const string Evaluador = "RequireEvaluadorRole";
        public static void AddAuthorizationPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(AdminPolicy, policy =>
                policy.RequireRole("Admin"));
            options.AddPolicy(Analista, policy =>
                policy.RequireRole("Analista", "Admin"));
            options.AddPolicy(Solicitante, policy =>
                policy.RequireRole("Solicitante", "Admin"));
            options.AddPolicy(Evaluador, policy =>
                policy.RequireRole("Evaluador", "Admin"));
        }
    }
}