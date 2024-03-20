namespace HPBank.DTOs
{
    public class UpdateCustomerFormFieldsDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? AddharNo { get; set; }
        public string? PanNo { get; set; }
        public DateTime? Dob { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? CustState { get; set; }
        public int Zip { get; set; }
    }
}
