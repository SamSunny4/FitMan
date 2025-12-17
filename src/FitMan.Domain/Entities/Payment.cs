using FitMan.Domain.Common;
using FitMan.Domain.Enums;

namespace FitMan.Domain.Entities;

public class Payment : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    
    public decimal Amount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalAmount { get; set; }
    
    public DateTime PaymentDate { get; set; }
    public DateTime DueDate { get; set; }
    
    public PaymentMethod PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    
    public PaymentType PaymentType { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    
    public string ReceiptNumber { get; set; } = string.Empty;
    public string? Notes { get; set; }
    
    public int? ProcessedByStaffId { get; set; }
    public Staff? ProcessedBy { get; set; }
    
    public bool IsOverdue => Status == PaymentStatus.Pending && DateTime.UtcNow > DueDate;
}
