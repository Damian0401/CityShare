namespace CityShare.Backend.Application.Core.Dtos.Auth;

public class ProfileDto
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int CreatedEvents { get; set; }
    public int ReceivedLikes { get; set; }
    public int GivenLikes { get; set; }
    public int ReceivedComments { get; set; }
    public int GivenComments { get; set; }
}
