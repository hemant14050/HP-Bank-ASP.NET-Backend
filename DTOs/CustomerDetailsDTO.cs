﻿namespace HPBank.DTOs
{
    public class CustomerDetailsDTO
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public string AddharNo { get; set; } = null!;
        public string PanNo { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? CustState { get; set; }
        public int Zip { get; set; }
        public long AccountNo { get; set; }
        public int AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
