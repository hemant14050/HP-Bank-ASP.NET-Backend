using AutoMapper;
using HPBank.DTOs;
using HPBank.DTOs.Response;
using HPBank.Helpers;
using HPBank.Models;
using HPBank.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HPBank.Repository
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly HPBankDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomersRepository> _logger;

        public CustomersRepository(HPBankDBContext dbContext, IMapper mapper, ILogger<CustomersRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        private async Task<ResponseDTO<CustomerDetailsDTO>> validateCustomerInputData(CustomerInputDTO data)
        {
            try
            {
                if (!EmailValidator.IsValid(data.Email))
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Invalid email id!"
                    };
                }
                Customer? cust = await _dbContext.Customers.FirstOrDefaultAsync(ct => ct.Email == data.Email);
                if (cust != null)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Email id should be unique. \nThis email id already exists with another customer!"
                    };
                }
                cust = await _dbContext.Customers.FirstOrDefaultAsync(ct => ct.AddharNo == data.AddharNo);
                if (cust != null)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Aadhar no should be unique. \nThis aadhar no already exists with another customer!"
                    };
                }
                cust = await _dbContext.Customers.FirstOrDefaultAsync(ct => ct.PanNo == data.PanNo);
                if (cust != null)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Pan no should be unique. \nThis PAN no already exists with another customer!"
                    };
                }
                //request is valid;
                return null;
            } 
            catch(Exception ex)
            {
                //invalid or db error
                return new ResponseDTO<CustomerDetailsDTO>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }

        public async Task<ResponseDTO<CustomerDetailsDTO>> AddCustAndCreateBankAccount(CustomerInputDTO data)
        {
            try
            {
                var isInvalid = await validateCustomerInputData(data);
                if(isInvalid != null)
                {
                    return isInvalid;
                }

                if(data.OpeningBalance <= 0)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Account opening balance should be greater than 0."
                    };
                }

                //Console.WriteLine(data);
                // Create a parameter list for the stored procedure
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@firstName", data.FirstName),
                    new SqlParameter("@lastName", data.LastName),
                    new SqlParameter("@email", data.Email),
                    new SqlParameter("@mobileNo", data.MobileNo),
                    new SqlParameter("@addharNo", data.AddharNo),
                    new SqlParameter("@panNo", data.PanNo),
                    new SqlParameter("@dob", data.Dob),
                    new SqlParameter("@street", data.Street),
                    new SqlParameter("@city", data.City),
                    new SqlParameter("@cust_state", data.CustState),
                    new SqlParameter("@zip", data.Zip),
                    new SqlParameter("@accountTypeId", data.AccountTypeId),
                    new SqlParameter("@openingBalance", data.OpeningBalance),
                    new SqlParameter
                    {
                        ParameterName = "@custId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    },
                    new SqlParameter
                    {
                        ParameterName = "@accountNo", 
                        SqlDbType = SqlDbType.BigInt,
                        Direction = ParameterDirection.Output
                    }
                };

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC usp_CreateBankAccountWithCustDetails " +
                    "@firstName, @lastName, @email, @mobileNo, @addharNo, @panNo, @dob, @street, @city, " +
                    "@cust_state, @zip, @accountTypeId, @openingBalance, @custId OUTPUT, @accountNo OUTPUT", 
                    parameters);

                int custId = (int)parameters[parameters.Length - 2].Value;
                long accountNo = (long)parameters[parameters.Length - 1].Value;

                var currCust = await GetCustomerById(custId);
                return new ResponseDTO<CustomerDetailsDTO>
                {
                    Success = true,
                    Message = $"Customer details added and account is created successfully! \n Customer account no is: {accountNo}",
                    Data = currCust.Data
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

        public async Task<ResponseDTO<List<CustomerDetailsDTO>>> GetAllCustomersDetails()
        {
            try
            {
                List<CustomerDetailsDTO>? custList = null;
                var joinData = from cust in _dbContext.Customers
                               join account in _dbContext.Accounts
                               on cust.CustomerId equals account.CustomerId
                               select new CustomerDetailsDTO
                               {
                                   CustomerId = cust.CustomerId,
                                   FirstName = cust.FirstName,
                                   LastName = cust.LastName,
                                   Email = cust.Email,
                                   MobileNo = cust.MobileNo,
                                   AddharNo = cust.AddharNo,
                                   PanNo = cust.PanNo,
                                   Dob = cust.Dob,
                                   Street = cust.Street,
                                   City = cust.City,
                                   CustState = cust.CustState,
                                   Zip = cust.Zip,
                                   AccountNo = account.AccountNo,
                                   AccountTypeId = account.AccountTypeId,
                                   Balance = account.Balance,
                                   IsActive = account.IsActive,
                                   CreatedAt = account.CreatedAt
                               };
                custList = await joinData.ToListAsync();
                if (custList == null)
                {
                    return new ResponseDTO<List<CustomerDetailsDTO>>
                    {
                        Success = false,
                        Message = "Customers not found!"
                    };
                }
                return new ResponseDTO<List<CustomerDetailsDTO>>
                {
                    Success = true,
                    Message = "All customers retrived successfully!",
                    Data = custList
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<List<CustomerDetailsDTO>>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }

        public async Task<ResponseDTO<CustomerDetailsDTO>> GetCustomerById(int customerId)
        {
            try
            {
                Customer? cust = await _dbContext.Customers.FirstOrDefaultAsync(cust => cust.CustomerId == customerId);
                if (cust == null)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Customer not found!"
                    };
                }

                Account? acc = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.CustomerId == customerId);
                if (acc == null)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Account not found!"
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
                    Message = "Customer retrived successfully!",
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

        public async Task<ResponseDTO<CustomerDetailsDTO>> GetCustomerByAddhar(string addharNo)
        {
            try
            {
                Customer? cust = await _dbContext.Customers.FirstOrDefaultAsync(cust => cust.AddharNo == addharNo);

                if (cust == null)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Customer not found!"
                    };
                }
                Account? acc = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.CustomerId == cust.CustomerId);
                if (acc == null)
                {
                    return new ResponseDTO<CustomerDetailsDTO>
                    {
                        Success = false,
                        Message = "Account not found!"
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
                    Message = "Customer retrived successfully!",
                    Data = custDt
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<CustomerDetailsDTO>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }

        public async Task<ResponseDTO<CustomerDTO>> DeleteCustById(int custId)
        {
            try
            {
                Customer? currCust = await _dbContext.Customers.FirstOrDefaultAsync(cust => cust.CustomerId == custId);
                if(currCust == null)
                {
                    return new ResponseDTO<CustomerDTO>
                    {
                        Success = false,
                        Message = "Customer not found."
                    };
                }
                _dbContext.Customers.Remove(currCust);
                await _dbContext.SaveChangesAsync();

                return new ResponseDTO<CustomerDTO>
                {
                    Success = true,
                    Message = "Customer deleted!",
                    Data = _mapper.Map<CustomerDTO>(currCust)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<CustomerDTO>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }

        public async Task<ResponseDTO<CustomerDTO>> UpdateCustomerById(int customerId, UpdateCustomerFormFieldsDTO updateFormData)
        {
            try
            {
                if(updateFormData.Email != null && !EmailValidator.IsValid(updateFormData.Email))
                {
                    return new ResponseDTO<CustomerDTO>
                    {
                        Success = false,
                        Message = "Invalid email id!"
                    };
                }

                Customer? currCust = await _dbContext.Customers.FirstOrDefaultAsync(cust => cust.CustomerId == customerId);
                if(currCust == null)
                {
                    return new ResponseDTO<CustomerDTO>
                    {
                        Success = false,
                        Message = "Customer not found."
                    };
                }
                //use of reflection
                //dynamically updates customer
                foreach(var prop in updateFormData.GetType().GetProperties())
                {
                    var value = prop.GetValue(updateFormData);
                    if(value != null)
                    {
                        var currProp = currCust.GetType().GetProperty(prop.Name);
                        if(currProp != null)
                        {
                            currProp.SetValue(currCust, value);
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();

                return new ResponseDTO<CustomerDTO>
                {
                    Success = true,
                    Message = "Customer updated successfully!",
                    Data = _mapper.Map<CustomerDTO>(currCust)
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseDTO<CustomerDTO>
                {
                    Success = false,
                    Message = "Something wents wrong!"
                };
            }
        }
    }
}
