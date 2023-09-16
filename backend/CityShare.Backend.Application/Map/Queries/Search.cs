using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos.Map;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries;

public class Search
{
    public record Query(string Phrase) : IRequest<Result<AddressDetailsDto>>;
    
    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Phrase).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Query, Result<AddressDetailsDto>>
    {
        private readonly INominatimService _nominatimService;
        private readonly IMapper _mapper;

        public Handler(INominatimService nominatimService, IMapper mapper)
        {
            _nominatimService = nominatimService;
            _mapper = mapper;
        }

        public async Task<Result<AddressDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _nominatimService.SearchByQueryAsync(request.Phrase, cancellationToken);

            if (result is null)
            {
                return Result<AddressDetailsDto>.Failure(Errors.NotFound);
            }

            var mapperResult = _mapper.Map<AddressDetailsDto>(result);

            return mapperResult;
        }
    }
}
