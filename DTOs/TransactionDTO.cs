namespace HPBank.DTOs
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public long AccountNo { get; set; }
        public string TransactionType { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal ClosingAmt { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
