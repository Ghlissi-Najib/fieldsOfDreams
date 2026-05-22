using System.Transactions;

namespace Thefieldofdreams.Domain.Entities
{
    

    public class Wallet : BaseEntity
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; } = 0;
        public int Points { get; set; } = 0;
        public string? StripeCustomerId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public class Transaction : BaseEntity
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
        public string? ReferenceId { get; set; }
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        public virtual Wallet Wallet { get; set; }
    }

    public enum TransactionType
    {
        Credit,
        Debit,
        Reward,
        Purchase,
        Refund
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed,
        Cancelled
    }
}