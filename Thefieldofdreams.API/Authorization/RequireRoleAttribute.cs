using Microsoft.AspNetCore.Authorization;

namespace Thefieldofdreams.API.Authorization
{
    public sealed class RequireRoleAttribute : AuthorizeAttribute
    {
        public RequireRoleAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}
