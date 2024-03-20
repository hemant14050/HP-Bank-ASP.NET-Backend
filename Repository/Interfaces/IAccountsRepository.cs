using HPBank.DTOs;
using HPBank.DTOs.Response;

namespace HPBank.Repository.Interfaces
{
    public interface IAccountsRepository
    {
        Task<ResponseDTO<List<AccountDTO>>> GetAllAccounts();
        Task<ResponseDTO<CustomerDetailsDTO>> GetAccountByAccNo(long accNo);
        Task<ResponseDTO<AccountDTO>> ChangeStatus(long accountNo);
        Task<ResponseDTO<InterestRateDTO>> GetInterestRate(int accountTypeId);
    }
}
