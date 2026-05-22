using System.ComponentModel.DataAnnotations;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
        public string? ReferenceId { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTransactionRequestDto
    {
        [Required]
        public Guid WalletId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(200)]
        public string? ReferenceId { get; set; }
    }
}
