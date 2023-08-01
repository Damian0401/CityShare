namespace CityShare.Backend.Api.Api;

public static class Endpoints
{
    public class V1
    {
        public class Auth
        {
            public const string Register = "/api/v1/auth/register";
            public const string Login = "/api/v1/auth/login";
            public const string Refresh = "/api/v1/auth/refresh";
        }

        public class Map
        {
            public const string Search = "/api/v1/map/search";
            public const string Address = "/api/v1/map/address";
        }
    }
}
