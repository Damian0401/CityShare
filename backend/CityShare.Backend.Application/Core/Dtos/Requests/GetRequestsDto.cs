namespace CityShare.Backend.Application.Core.Dtos.Requests;

public record GetRequestsDto(IDictionary<int, IEnumerable<RequestDto>> Requests);
