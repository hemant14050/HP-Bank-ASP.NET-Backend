using AutoMapper;
using HPBank.DTOs;
using HPBank.DTOs.Response;
using HPBank.Models;
using HPBank.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HPBank.Repository
{
    public class TransactionsRepository: ITransactionsRepository
    {
        private readonly HPBankDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsRepository> _logger;

        public TransactionsRepository(HPBankDBContext dbContext, IMapper mapper, ILogger<TransactionsRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDTO<TransactionDTO>> InitiateTransaction(TransactionFormDTO transactionFormData)
        {
            try
            {
                long accountNo = transactionFormData.AccountNo;
                string transactionType = transactionFormData.TransactionType;
                decimal amount = transactionFormData.Amount;

                Account? acc = await _dbContext.Accounts.FirstOrDefaultAsync(ac => ac.AccountNo == accountNo);
                if(acc == null)
                {
                    return new ResponseDTO<TransactionDTO>
                    {
                        Success = false,
                        Message = $"Invalid account no: {accountNo}"
                    };
                }

                if(acc.IsActive == false)
                {
                    return new ResponseDTO<TransactionDTO>
                    {
                        Success = false,
                        Message = "This account is inactive. \nPlease activate it to make transactions."
                    };
                }

                if (!(transactionType == "Withdraw" || transactionType == "Deposit"))
                {
                    return new ResponseDTO<TransactionDTO>
                    {
                        Success = false,
                        Message = "Invalid transaction type."
                    };
                }

                if(amount <= 0)
                {
                    return new ResponseDTO<TransactionDTO>
                    {
                        Success = false,
                        Message = "Invalid amount."
                    };
                }

                if(transactionType == "Withdraw" && (acc.Balance - amount) < 0)
                {
                    return new ResponseDTO<TransactionDTO>
                    {
                        Success = false,
                        Message = $"Insufficient balance."
                    };
                }

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@accountNo", accountNo),
                    new SqlParameter("@transactionType", transactionType),
                    new SqlParameter("@amount", amount),
                    new SqlParameter
                    {
                        ParameterName = "@currTransactionId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    }
                };
                await _dbContext.Database.ExecuteSqlRawAsync("EXEC usp_InitiateTransaction @accountNo, @transactionType, @amount, @currTransactionId OUTPUT", parameters);
                int currTransactionId = (int)parameters[parameters.Length - 1].Value;

                AccountTransaction? currTransaction = await _dbContext.AccountTransactions.FirstOrDefaultAsync(t => t.TransactionId == currTransactionId);

                return new ResponseDTO<TransactionDTO>
                {
                    Success = true,
                    Message = $"Amount ₹{amount} {transactionType} successfully for account no: {accountNo}",
                    Data = _mapper.Map<TransactionDTO>(currTransaction)
                };

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<TransactionDTO>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }

        public async Task<ResponseDTO<List<TransactionDTO>>> GetAllTransactions()
        {
            try
            {
                List<TransactionDTO> tranList = await _dbContext.AccountTransactions.Select(t => _mapper.Map<TransactionDTO>(t)).ToListAsync();
                return new ResponseDTO<List<TransactionDTO>>
                {
                    Success = true,
                    Message = "Transactions list retrived successfully!",
                    Data = tranList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<List<TransactionDTO>>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }

        public async Task<ResponseDTO<List<TransactionDTO>>> GetTrasactionsByAccountNo(long accNo)
        {
            try
            {
                var currAcc = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.AccountNo == accNo);
                if(currAcc == null)
                {
                    return new ResponseDTO<List<TransactionDTO>>
                    {
                        Success = false,
                        Message = "Invalid account no."
                    };
                }
                var tranList = from transaction in _dbContext.AccountTransactions
                               where transaction.AccountNo == accNo
                               select _mapper.Map<TransactionDTO>(transaction);

                List<TransactionDTO> currList = await tranList.ToListAsync();
                return new ResponseDTO<List<TransactionDTO>>
                {
                    Success = true,
                    Message = "Transactions retrived successfully!",
                    Data = currList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<List<TransactionDTO>>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }
    }
}
