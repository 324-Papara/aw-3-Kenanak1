using System.Text.Json.Serialization;
using Para.Base.Schema;

namespace Para.Schema;

public class CustomerRequest  : BaseRequest
{
    [JsonIgnore]
    public int CustomerNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class CustomerResponse : BaseResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdentityNumber { get; set; }
    public string Email { get; set; }
    public int CustomerNumber { get; set; }
    public DateTime DateOfBirth { get; set; }

    public CustomerDetailsResponse CustomerDetails { get; set; }
    public List<CustomerAddressResponse> CustomerAddresses { get; set; }
    public List<CustomerPhoneResponse> CustomerPhones { get; set; }
}
public class CustomerDetailsResponse
{
    public string FatherName { get; set; }
    public string MotherName { get; set; }
    public string EducationStatus { get; set; }
    public string MonthlyIncome { get; set; }
    public string Occupation { get; set; }
}