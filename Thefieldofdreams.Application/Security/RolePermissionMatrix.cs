using System.Security.Claims;

namespace Thefieldofdreams.Application.Security
{
    public static class RolePermissionMatrix
    {
        private static readonly IReadOnlyDictionary<UserRole, Permission> PermissionsByRole = new Dictionary<UserRole, Permission>
        {
            [UserRole.Admin] =
                Permission.ViewAllBookings |
                Permission.ViewOwnBookings |
                Permission.PostLiveSlots |
                Permission.ViewAggregateAnalytics |
                Permission.ViewFullAnalytics |
                Permission.PlayGames |
                Permission.ValidatePerks |
                Permission.ManageUsers |
                Permission.ManagePartners |
                Permission.ManageRewards |
                Permission.AccessGoogleSheetsDashboard |
                Permission.ControlMakeScenarios |
                Permission.SelectWinners |
                Permission.ViewOwnTicketsAndRewards,

            [UserRole.Merchant] =
                Permission.ViewOwnBookings |
                Permission.PostLiveSlots |
                Permission.ValidatePerks,

            [UserRole.Partner] =
                Permission.ViewAggregateAnalytics,

            [UserRole.Passenger] =
                Permission.PlayGames |
                Permission.ViewOwnTicketsAndRewards |
                Permission.NfcCheckIn |
                Permission.EnterGiveaway
        };

        public static Permission GetPermissions(UserRole role) =>
            PermissionsByRole.TryGetValue(role, out var permissions) ? permissions : Permission.None;

        public static bool HasPermission(UserRole role, Permission permission) =>
            (GetPermissions(role) & permission) == permission;

        public static bool HasPermission(ClaimsPrincipal user, Permission permission)
        {
            var roleClaims = user.FindAll(ClaimTypes.Role).Select(c => c.Value);
            foreach (var roleClaim in roleClaims)
            {
                if (Enum.TryParse<UserRole>(roleClaim, ignoreCase: true, out var role) && HasPermission(role, permission))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
