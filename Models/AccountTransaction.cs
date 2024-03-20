using System;
using System.Collections.Generic;

namespace HPBank.Models
{
    public partial class AccountTransaction
    {
        public int TransactionId { get; set; }
        public long AccountNo { get; set; }
        public string TransactionType { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal ClosingAmt { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Account AccountNoNavigation { get; set; } = null!;
    }
}
