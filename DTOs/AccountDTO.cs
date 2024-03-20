namespace HPBank.DTOs
{
    public class AccountDTO
    {
        public long AccountNo { get; set; }
        public int CustomerId { get; set; }
        public int AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
