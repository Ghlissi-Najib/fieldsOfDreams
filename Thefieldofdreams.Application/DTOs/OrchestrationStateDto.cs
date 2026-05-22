using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class OrchestrationStateDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
        public int UserPoints { get; set; }
        public List<ActiveCampaignSummaryDto> ActiveCampaigns { get; set; } = [];
        public List<Guid> VisibleRewardIds { get; set; } = [];
        public List<Guid> ValidQRCodeIds { get; set; } = [];
        public Dictionary<string, bool> EnabledFlows { get; set; } = [];
    }

    public class ActiveCampaignSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CampaignType Type { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
    }
}
