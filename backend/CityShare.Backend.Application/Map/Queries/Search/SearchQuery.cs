using CityShare.Backend.Application.Core.Dtos;
using CityShare.Backend.Application.Core.Models.Map.Search;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Map.Queries.Search;

public record SearchQuery(string Query) : IRequest<Result<MapSearchResponseModel>>;
