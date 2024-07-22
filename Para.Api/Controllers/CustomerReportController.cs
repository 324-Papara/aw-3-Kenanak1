using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Para.Data.Context;
using Para.Schema;

namespace Para.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerReportController : ControllerBase
    {
        private readonly ParaDbContext _dbContext;

        public CustomerReportController(ParaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerReport(long customerId)
        {
            const string sql = @"
                SELECT 
                    c.CustomerId, c.FirstName, c.LastName, c.IdentityNumber, c.Email, c.CustomerNumber, c.DateOfBirth,
                    a.Country, a.City, a.AddressLine, a.ZipCode, a.IsDefault AS IsDefaultAddress,
                    d.FatherName, d.MotherName, d.EducationStatus, d.MonthlyIncome, d.Occupation,
                    p.CountyCode, p.Phone, p.IsDefault AS IsDefaultPhone
                FROM Customers c
                LEFT JOIN CustomerAddress a ON c.CustomerId = a.CustomerId
                LEFT JOIN CustomerDetail d ON c.CustomerId = d.CustomerId
                LEFT JOIN CustomerPhone p ON c.CustomerId = p.CustomerId
                WHERE c.CustomerId = @CustomerId
            ";

            using (var connection = _dbContext.Database.GetDbConnection())
            {
                var customerData = await connection.QueryAsync<CustomerResponse, CustomerAddressResponse, CustomerDetailResponse, CustomerPhoneResponse, CustomerResponse>(
                    sql,
                    (customer, address, detail, phone) =>
                    {
                        if (customer.CustomerAddresses == null)
                            customer.CustomerAddresses = new List<CustomerAddressResponse>();

                        if (customer.CustomerPhones == null)
                            customer.CustomerPhones = new List<CustomerPhoneResponse>();

                        // Add address and phone details
                        if (address != null)
                            customer.CustomerAddresses.Add(address);
                        if (phone != null)
                            customer.CustomerPhones.Add(phone);

                        return customer;
                    },
                    new { CustomerId = customerId },
                    splitOn: "Country, FatherName, CountyCode");

                var customerReport = customerData.FirstOrDefault();

                if (customerReport == null)
                {
                    return NotFound();
                }

                // Ensure CustomerDetails is populated
                if (customerReport.CustomerDetails == null)
                {
                    customerReport.CustomerDetails = new CustomerDetailsResponse();
                }

                return Ok(customerReport);
            }
        }
    }
}
