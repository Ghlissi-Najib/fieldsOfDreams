using Thefieldofdreams.Application.Security;

namespace Thefieldofdreams.API.Authorization
{
    public static class RbacPolicies
    {
        public const string ViewAllBookings = "Permission.ViewAllBookings";
        public const string ViewOwnBookings = "Permission.ViewOwnBookings";
        public const string PostLiveSlots = "Permission.PostLiveSlots";
        public const string ViewAggregateAnalytics = "Permission.ViewAggregateAnalytics";
        public const string ViewFullAnalytics = "Permission.ViewFullAnalytics";
        public const string PlayGames = "Permission.PlayGames";
        public const string ValidatePerks = "Permission.ValidatePerks";
        public const string ManageUsers = "Permission.ManageUsers";
        public const string ManagePartners = "Permission.ManagePartners";
        public const string ManageRewards = "Permission.ManageRewards";
        public const string AccessGoogleSheetsDashboard = "Permission.AccessGoogleSheetsDashboard";
        public const string ControlMakeScenarios = "Permission.ControlMakeScenarios";
        public const string SelectWinners = "Permission.SelectWinners";
        public const string ViewOwnTicketsAndRewards = "Permission.ViewOwnTicketsAndRewards";
        public const string NfcCheckIn = "Permission.NfcCheckIn";
        public const string EnterGiveaway = "Permission.EnterGiveaway";

        public static IReadOnlyDictionary<string, Permission> All => new Dictionary<string, Permission>
        {
            [ViewAllBookings] = Permission.ViewAllBookings,
            [ViewOwnBookings] = Permission.ViewOwnBookings,
            [PostLiveSlots] = Permission.PostLiveSlots,
            [ViewAggregateAnalytics] = Permission.ViewAggregateAnalytics,
            [ViewFullAnalytics] = Permission.ViewFullAnalytics,
            [PlayGames] = Permission.PlayGames,
            [ValidatePerks] = Permission.ValidatePerks,
            [ManageUsers] = Permission.ManageUsers,
            [ManagePartners] = Permission.ManagePartners,
            [ManageRewards] = Permission.ManageRewards,
            [AccessGoogleSheetsDashboard] = Permission.AccessGoogleSheetsDashboard,
            [ControlMakeScenarios] = Permission.ControlMakeScenarios,
            [SelectWinners] = Permission.SelectWinners,
            [ViewOwnTicketsAndRewards] = Permission.ViewOwnTicketsAndRewards,
            [NfcCheckIn] = Permission.NfcCheckIn,
            [EnterGiveaway] = Permission.EnterGiveaway
        };
    }
}
