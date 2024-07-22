using MediatR;
using Para.Base.Response;
using Para.Schema;

namespace Para.Bussiness.Cqrs;

public record CreateCustomerDetailCommand(CustomerDetailRequest Request) : IRequest<ApiResponse<CustomerDetailResponse>>;
public record UpdateCustomerDetailCommand(long DetailId, CustomerDetailRequest Request) : IRequest<ApiResponse>;
public record DeleteCustomerDetailCommand(long DetailId) : IRequest<ApiResponse>;

public record GetAllCustomerDetailsQuery() : IRequest<ApiResponse<List<CustomerDetailResponse>>>;
public record GetCustomerDetailByIdQuery(long DetailId) : IRequest<ApiResponse<CustomerDetailResponse>>;