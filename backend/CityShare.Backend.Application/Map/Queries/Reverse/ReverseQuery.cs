using CityShare.Backend.Application.Core.Dtos.Map;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries.Reverse;

public record ReverseQuery(double X, double Y) : IRequest<Result<AddressDto>>;