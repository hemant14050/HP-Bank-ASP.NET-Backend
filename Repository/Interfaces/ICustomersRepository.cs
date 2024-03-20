using HPBank.DTOs.Response;
using HPBank.DTOs;
using HPBank.Models;

namespace HPBank.Repository.Interfaces
{
    public interface ICustomersRepository
    {
        Task<ResponseDTO<CustomerDetailsDTO>> AddCustAndCreateBankAccount(CustomerInputDTO data);
        Task<ResponseDTO<List<CustomerDetailsDTO>>> GetAllCustomersDetails();
        Task<ResponseDTO<CustomerDetailsDTO>> GetCustomerById(int customerId);
        Task<ResponseDTO<CustomerDetailsDTO>> GetCustomerByAddhar(string addharNo);
        Task<ResponseDTO<CustomerDTO>> UpdateCustomerById(int customerId, UpdateCustomerFormFieldsDTO updateFormData);
        Task<ResponseDTO<CustomerDTO>> DeleteCustById(int custId);
    }
}
