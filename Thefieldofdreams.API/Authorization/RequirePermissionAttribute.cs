using Microsoft.AspNetCore.Authorization;

namespace Thefieldofdreams.API.Authorization
{
    public sealed class RequirePermissionAttribute : AuthorizeAttribute
    {
        public RequirePermissionAttribute(string policy)
        {
            Policy = policy;
        }
    }
}
