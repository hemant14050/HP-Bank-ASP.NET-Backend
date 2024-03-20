using HPBank.DTOs.Response;
using HPBank.DTOs;

namespace HPBank.Repository.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<ResponseDTO<TransactionDTO>> InitiateTransaction(TransactionFormDTO transactionFormData);
        Task<ResponseDTO<List<TransactionDTO>>> GetAllTransactions();
        Task<ResponseDTO<List<TransactionDTO>>> GetTrasactionsByAccountNo(long accNo);
    }
}
