using HPBank.DTOs;
using HPBank.DTOs.Response;
using HPBank.Models;
using HPBank.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HPBank.Controllers
{
    [ApiController]
    [Route("api/v1/customers")]
    public class CustomersController : Controller
    {
        private readonly ICustomersRepository _customersRepository;

        public CustomersController(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        // POST - api/v1/customers
        [HttpPost]
        public async Task<ActionResult<ResponseDTO<CustomerDetailsDTO>>> AddCustAndCreateBankAccount([FromBody] CustomerInputDTO reqData)
        {
            ResponseDTO<CustomerDetailsDTO> res = await _customersRepository.AddCustAndCreateBankAccount(reqData);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        // GET - api/v1/customers
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<List<CustomerDetailsDTO>>>> GetAllCustomersDetails()
        {
            ResponseDTO<List<CustomerDetailsDTO>> res = await _customersRepository.GetAllCustomersDetails();
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }
        }

        // GET - api/v1/customers/{customerId}
        [HttpGet("{customerId:int}")]
        public async Task<ActionResult<ResponseDTO<CustomerDetailsDTO>>> GetCustomerById([FromRoute] int customerId)
        {
            ResponseDTO<CustomerDetailsDTO> res = await _customersRepository.GetCustomerById(customerId);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }
        }

        // GET - api/v1/customers/customer?aadharNo=""
        [HttpGet("customer")]
        public async Task<ActionResult<ResponseDTO<CustomerDetailsDTO>>> GetCustomerAddharNo([FromQuery] string aadharNo)
        {
            ResponseDTO<CustomerDetailsDTO> res = await _customersRepository.GetCustomerByAddhar(aadharNo);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return NotFound(res);
            }
        }

        // PATCH - api/v1/customers/{customerId}
        [HttpPatch("{customerId:int}")]
        public async Task<ActionResult<ResponseDTO<CustomerDTO>>> UpdateCustomerById([FromRoute] int customerId, [FromBody] UpdateCustomerFormFieldsDTO updateFormData)
        {
            ResponseDTO<CustomerDTO> res = await _customersRepository.UpdateCustomerById(customerId, updateFormData);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }

        // DELETE - api/v1/customers/{customerId}
        [HttpDelete("{customerId:int}")]
        public async Task<ActionResult<ResponseDTO<CustomerDTO>>> DeleteCustById([FromRoute] int customerId)
        {
            ResponseDTO<CustomerDTO> res = await _customersRepository.DeleteCustById(customerId);
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
