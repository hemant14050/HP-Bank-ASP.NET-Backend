using HPBank.DTOs;

namespace HPBank.Repository.Interfaces
{
    public interface IInterestRateRepository
    {
        int AccountTypeId { get; }
        Task<InterestRateDTO> GetInterestRate();
    }
}
