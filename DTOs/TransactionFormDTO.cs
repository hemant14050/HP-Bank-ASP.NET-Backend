namespace HPBank.DTOs
{
    public class TransactionFormDTO
    {
        public long AccountNo { get; set; }
        public string TransactionType { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
