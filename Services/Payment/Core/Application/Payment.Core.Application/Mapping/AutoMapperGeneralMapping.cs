using AutoMapper;
using Payment.Core.Application.CQRS.Command.Authorize;
using Payment.Core.Application.CQRS.Query.GetTransactionsByFilter;
using Payment.Core.Domain.Entities;

namespace Payment.Core.Application.Mapping
{
    public class AutoMapperGeneralMapping : Profile
    {
        public AutoMapperGeneralMapping()
        {
            CreateMap<AuthorizeCommand, Transaction>().ReverseMap();

            CreateMap<TransactionModel, Transaction>().ReverseMap();
        }
    }
}
