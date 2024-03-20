using AutoMapper;
using HPBank.DTOs;
using HPBank.Models;
using System.Transactions;

namespace HPBank.Helpers
{
    public class ApplicationModelMapping: Profile
    {
        public ApplicationModelMapping()
        {
            CreateMap<Customer, CustomerDetailsDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<TransactionDTO, Transaction>().ReverseMap();
            CreateMap<InterestRateDTO, AccountType>().ReverseMap();
        }
    }
}
