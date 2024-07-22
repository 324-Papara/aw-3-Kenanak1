using AutoMapper;
using Para.Data.Domain;
using Para.Schema;

namespace Para.Bussiness;

public class MapperConfig : Profile
{

    public MapperConfig()
    {
        CreateMap<Customer, CustomerResponse>()
            .ForMember(dest => dest.CustomerDetails, opt => opt.MapFrom(src => src.CustomerDetail))
            .ForMember(dest => dest.CustomerAddresses, opt => opt.MapFrom(src => src.CustomerAddresses))
            .ForMember(dest => dest.CustomerPhones, opt => opt.MapFrom(src => src.CustomerPhones));


        CreateMap<Customer, CustomerResponse>();
        CreateMap<CustomerRequest, Customer>();
        
        CreateMap<CustomerAddress, CustomerAddressResponse>();
        CreateMap<CustomerAddressRequest, CustomerAddress>();
        
        CreateMap<CustomerPhone, CustomerPhoneResponse>();
        CreateMap<CustomerPhoneRequest, CustomerPhone>();
        
        CreateMap<CustomerDetail, CustomerDetailResponse>();
        CreateMap<CustomerDetailRequest, CustomerDetail>();
    }
}