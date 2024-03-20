using AutoMapper;
using HPBank.DTOs;
using HPBank.DTOs.Response;
using HPBank.Models;
using HPBank.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HPBank.Repository
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly HPBankDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountsRepository> _logger;
        private readonly IEnumerable<IInterestRateRepository> _interestRateCalculators;

        public AccountsRepository(HPBankDBContext dbContext, IMapper mapper, ILogger<AccountsRepository> logger, IEnumerable<IInterestRateRepository> interestRateCalculators)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _interestRateCalculators = interestRateCalculators;
        }
        public async Task<ResponseDTO<CustomerDetailsDTO>> GetAccountByAccNo(long accNo)
        {
            try
            {
                Account? acc = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.AccountNo == accNo);
                if (acc == null)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Account not found!"
                    };
                }
                Customer? cust = await _dbContext.Customers.FirstOrDefaultAsync(cust => cust.CustomerId == acc.CustomerId);
                if (cust == null)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Customer not found!"
                    };
                }


                CustomerDetailsDTO custDt = _mapper.Map<CustomerDetailsDTO>(cust);
                custDt.AccountNo = acc.AccountNo;
                custDt.AccountTypeId = acc.AccountTypeId;
                custDt.Balance = acc.Balance;
                custDt.IsActive = acc.IsActive;

                return new ResponseDTO<CustomerDetailsDTO>
                {
                    Success = true,
                    Message = "Customer and account retrived successfully!",
                    Data = custDt
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<CustomerDetailsDTO>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }

        public async Task<ResponseDTO<List<AccountDTO>>> GetAllAccounts()
        {
            try
            {
                var accList = await _dbContext.Accounts.Select(acc => _mapper.Map<AccountDTO>(acc)).ToListAsync();
                return new ResponseDTO<List<AccountDTO>>
                {
                    Success = true,
                    Message = "All accounts retrived successfully.",
                    Data = accList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<List<AccountDTO>>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }

        public async Task<ResponseDTO<AccountDTO>> ChangeStatus(long accountNo)
        {
            try
            {
                Account? currAcc = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.AccountNo == accountNo);
                if (currAcc == null)
                {
                    return new ResponseDTO<AccountDTO>
                    {
                        Success = false,
                        Message = "Account not found!"
                    };
                }
                currAcc.IsActive = !currAcc.IsActive;
                await _dbContext.SaveChangesAsync();

                AccountDTO currAccDto = _mapper.Map<AccountDTO>(currAcc);
                return new ResponseDTO<AccountDTO>
                {
                    Success = true,
                    Message = $"Account status updated to isActive:{currAcc.IsActive} successfully.",
                    Data = currAccDto
                };
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<AccountDTO>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }

        public async Task<ResponseDTO<InterestRateDTO>> GetInterestRate(int accountTypeId)
        {
            try
            {
                var currService = _interestRateCalculators.FirstOrDefault(iService => iService.AccountTypeId == accountTypeId);
                if (currService == null)
                {
                    return new ResponseDTO<InterestRateDTO>
                    {
                        Success = false,
                        Message = "Service not found or Invalid account type id."
                    };
                }
                InterestRateDTO resData = await currService.GetInterestRate();
                return new ResponseDTO<InterestRateDTO>
                {
                    Success = true,
                    Message = "Rate retrived successfully!",
                    Data = resData
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<InterestRateDTO>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }
    }
}
