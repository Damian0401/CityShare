namespace CityShare.Backend.Application.Core.Dtos.Requests;

public record GetRequestsResponseDto(IDictionary<int, IEnumerable<RequestDto>> Requests);
