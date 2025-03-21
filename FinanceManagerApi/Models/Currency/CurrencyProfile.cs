using AutoMapper;

namespace FinanceManagerApi.Models.Currency
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Entities.Currency, CurrencyDto>();
        }
    }
}
