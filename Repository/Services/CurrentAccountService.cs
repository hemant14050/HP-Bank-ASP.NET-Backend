using HPBank.DTOs;
using HPBank.Models;
using HPBank.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HPBank.Repository.Services
{
    public class CurrentAccountService : IInterestRateRepository
    {
        private readonly HPBankDBContext _dbContext;
        public int AccountTypeId { get { return 2; } }

        public CurrentAccountService(HPBankDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<InterestRateDTO> GetInterestRate()
        {
            var interestType = await _dbContext.AccountTypes.FirstOrDefaultAsync(accType => accType.AccountTypeId == AccountTypeId);
            return new InterestRateDTO
            {
                AccountTypeId = AccountTypeId,
                InterestRate = interestType.InterestRate
            };
        }
    }
}
