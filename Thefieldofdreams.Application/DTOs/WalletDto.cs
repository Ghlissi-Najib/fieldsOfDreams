namespace Thefieldofdreams.Application.DTOs
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public int Points { get; set; }
        public string? StripeCustomerId { get; set; }
    }
}
