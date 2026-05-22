using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Thefieldofdreams.API.Authorization;
using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>Legacy summary – minimal counts.</summary>
        [RequirePermission(RbacPolicies.ViewAggregateAnalytics)]
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _dashboardService.GetSummaryAsync();
            return Ok(summary);
        }

        /// <summary>Full admin dashboard with charts data.</summary>
        [RequirePermission(RbacPolicies.ViewFullAnalytics)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var data = await _dashboardService.GetAdminDashboardAsync();
            return Ok(data);
        }

        /// <summary>Passenger personal stats dashboard.</summary>
        [HttpGet("passenger")]
        public async Task<IActionResult> GetPassengerDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guid))
                return Unauthorized();

            var data = await _dashboardService.GetPassengerDashboardAsync(guid);
            return Ok(data);
        }

        /// <summary>Merchant QR / campaign performance dashboard.</summary>
        [RequirePermission(RbacPolicies.ValidatePerks)]
        [HttpGet("merchant")]
        public async Task<IActionResult> GetMerchantDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guid))
                return Unauthorized();

            var data = await _dashboardService.GetMerchantDashboardAsync(guid);
            return Ok(data);
        }

        /// <summary>Partner aggregate view (same as admin minus user management data).</summary>
        [RequirePermission(RbacPolicies.ViewAggregateAnalytics)]
        [HttpGet("partner")]
        public async Task<IActionResult> GetPartnerDashboard()
        {
            // Partners get the admin dashboard with full analytics read-only
            var data = await _dashboardService.GetAdminDashboardAsync();
            return Ok(data);
        }
    }
}
