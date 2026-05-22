using System;

namespace Thefieldofdreams.Application.Security
{
    [Flags]
    public enum Permission
    {
        None = 0,
        ViewAllBookings = 1 << 0,
        ViewOwnBookings = 1 << 1,
        PostLiveSlots = 1 << 2,
        ViewAggregateAnalytics = 1 << 3,
        ViewFullAnalytics = 1 << 4,
        PlayGames = 1 << 5,
        ValidatePerks = 1 << 6,
        ManageUsers = 1 << 7,
        ManagePartners = 1 << 8,
        ManageRewards = 1 << 9,
        AccessGoogleSheetsDashboard = 1 << 10,
        ControlMakeScenarios = 1 << 11,
        SelectWinners = 1 << 12,
        ViewOwnTicketsAndRewards = 1 << 13,
        NfcCheckIn = 1 << 14,
        EnterGiveaway = 1 << 15
    }
}
