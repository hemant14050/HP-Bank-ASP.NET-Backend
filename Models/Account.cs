using System;
using System.Collections.Generic;

namespace HPBank.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountTransactions = new HashSet<AccountTransaction>();
        }

        public long AccountNo { get; set; }
        public int CustomerId { get; set; }
        public int AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual AccountType AccountType { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }
    }
}
