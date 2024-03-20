using HPBank.DTOs;
using HPBank.DTOs.Response;
using HPBank.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HPBank.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountsController : Controller
    {
        private readonly IAccountsRepository _accountsRepository;

        public AccountsController(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO<List<AccountDTO>>>> GetAllAccounts()
        {
            ResponseDTO<List<AccountDTO>> res = await _accountsRepository.GetAllAccounts();
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }
        }

        [HttpGet("{accountNo:long}")]
        public async Task<ActionResult<ResponseDTO<CustomerDetailsDTO>>> GetAccountById([FromRoute] long accountNo)
        {
            ResponseDTO<CustomerDetailsDTO> res = await _accountsRepository.GetAccountByAccNo(accountNo);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }
        }

        [HttpPatch("changeStatus/{accountNo:long}")]
        public async Task<ActionResult<ResponseDTO<AccountDTO>>> ChangeStatus([FromRoute] long accountNo)
        {
            ResponseDTO<AccountDTO> res = await _accountsRepository.ChangeStatus(accountNo);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }
        }


        [HttpGet("getInterestRate/{accountTypeId:int}")]
        public async Task<ActionResult<ResponseDTO<InterestRateDTO>>> GetInterestRate([FromRoute] int accountTypeId)
        {
            ResponseDTO<InterestRateDTO> res = await _accountsRepository.GetInterestRate(accountTypeId);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }
        }
    }
}
