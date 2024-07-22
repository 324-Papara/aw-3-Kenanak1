using MediatR;
using Para.Base.Response;
using Para.Schema;

namespace Para.Bussiness.Cqrs;

public record CreateCustomerPhoneCommand(CustomerPhoneRequest Request) : IRequest<ApiResponse<CustomerPhoneResponse>>;
public record UpdateCustomerPhoneCommand(long PhoneId, CustomerPhoneRequest Request) : IRequest<ApiResponse>;
public record DeleteCustomerPhoneCommand(long PhoneId) : IRequest<ApiResponse>;

public record GetAllCustomerPhonesQuery() : IRequest<ApiResponse<List<CustomerPhoneResponse>>>;
public record GetCustomerPhoneByIdQuery(long PhoneId) : IRequest<ApiResponse<CustomerPhoneResponse>>;
