namespace CityShare.Services.Api.Endpoints;

public static class Endpoints
{
    public class V1
    {
        public class Auth
        {
            public const string Index = "/api/v1/auth";
            public const string Register = "/api/v1/auth/register";
            public const string Login = "/api/v1/auth/login";
            public const string Refresh = "/api/v1/auth/refresh";
            public const string ConfirmEmail = "/api/v1/auth/confirm-email";
        }

        public class Map
        {
            public const string Index = "/api/v1/map";
            public const string Search = "/api/v1/map/search";
            public const string Reverse = "/api/v1/map/reverse";
        }

        public class Cities
        {
            public const string Index = "/api/v1/cities";
        }

        public class Categories
        {
            public const string Index = "/api/v1/categories";
        }

        public class Events
        {
            public const string Index = "/api/v1/events";

            public class Id
            {
                public const string Index = "/api/v1/events/{id}";
                public const string Images = "/api/v1/events/{id}/images";
                public const string Likes = "/api/v1/events/{id}/likes";
                public const string Comments = "/api/v1/events/{id}/comments";
            }
        }

        public class Requests
        {
            public const string Index = "/api/v1/requests";
            public const string Types = "/api/v1/requests/types";
        }
    }

    public class Hubs
    {
        public const string Comments = "/hubs/comments";
    }
}
