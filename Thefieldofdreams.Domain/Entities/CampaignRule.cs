namespace Thefieldofdreams.Domain.Entities
{
    public class CampaignRule : BaseEntity
    {
        public Guid CampaignId { get; set; }
        public CampaignRuleType RuleType { get; set; }
        public string ParametersJson { get; set; } = "{}";

        public virtual Campaign Campaign { get; set; } = null!;
    }

    public enum CampaignRuleType
    {
        RequireRole,       // {"role":"passenger"}
        MinimumPoints,     // {"points":100}
        MaxScansPerUser,   // {"max":3}
        TargetUserGroup,   // {"group":"VIP"}
        TimeWindow,        // {"startHour":9,"endHour":18}
        GeoZone,           // {"wkt":"POLYGON((lon lat,...))"}  — ST_Within check
        PortArrival,       // {"portLocationId":"guid"}        — cruise port-day trigger
    }
}
