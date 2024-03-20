using HPBank.DTOs;
using HPBank.Models;
using HPBank.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HPBank.Repository.Services
{
    public class SavingAccountService : IInterestRateRepository
    {
        private readonly HPBankDBContext _dbContext;
        public int AccountTypeId { get { return 1; } }

        public SavingAccountService(HPBankDBContext dbContext)
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
