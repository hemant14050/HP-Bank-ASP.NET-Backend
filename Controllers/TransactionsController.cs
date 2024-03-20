using HPBank.DTOs;
using HPBank.DTOs.Response;
using HPBank.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HPBank.Controllers
{
    [ApiController]
    [Route("api/v1/transactions")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public TransactionsController(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        // GET - api/v1/transactions
        [HttpGet]
        public async Task<ActionResult<List<TransactionDTO>>> GetAllTransactions()
        {
            ResponseDTO<List<TransactionDTO>> res = await _transactionsRepository.GetAllTransactions();
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }
        }

        // GET - api/v1/transactions/makeTransaction
        [HttpPost("makeTransaction")]
        public async Task<ActionResult<List<TransactionDTO>>> InitiateTransaction([FromBody] TransactionFormDTO transactionFormData)
        {
            ResponseDTO<TransactionDTO> res = await _transactionsRepository.InitiateTransaction(transactionFormData);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        [HttpGet("getByAccountNo/{accountNo:long}")]
        public async Task<ActionResult<List<TransactionDTO>>> GetTrasactionsByAccountNo([FromRoute] long accountNo)
        {
            ResponseDTO<List<TransactionDTO>> res = await _transactionsRepository.GetTrasactionsByAccountNo(accountNo);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }
    }
}
