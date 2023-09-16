using AutoMapper;
using CityShare.Backend.Application.Core.Abstractions.Nominatim;
using CityShare.Backend.Application.Core.Dtos.Map;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries;

public class Reverse
{
    public record Query(double X, double Y) : IRequest<Result<AddressDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.X).NotEmpty();
            RuleFor(x => x.Y).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Query, Result<AddressDto>>
    {
        private readonly INominatimService _nominatimService;
        private readonly IMapper _mapper;

        public Handler(INominatimService nominatimService, IMapper mapper)
        {
            _nominatimService = nominatimService;
            _mapper = mapper;
        }

        public async Task<Result<AddressDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _nominatimService.ReverseAsync(request.X, request.Y, cancellationToken);

            if (result is null)
            {
                return Result<AddressDto>.Failure(Errors.NotFound);
            }

            var mapperResult = _mapper.Map<AddressDto>(result);

            return mapperResult;
        }
    }
}
