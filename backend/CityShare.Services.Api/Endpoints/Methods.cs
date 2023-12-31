﻿using CityShare.Backend.Domain.Constants;

namespace CityShare.Services.Api.Endpoints;

public static class Methods
{
    public static WebApplication UseEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");

        app.MapPost(Endpoints.V1.Auth.Register, V1.Auth.Register)
            .AllowAnonymous();

        app.MapPost(Endpoints.V1.Auth.Login, V1.Auth.Login)
            .AllowAnonymous();

        app.MapPost(Endpoints.V1.Auth.Refresh, V1.Auth.Refresh)
            .AllowAnonymous();

        app.MapPost(Endpoints.V1.Auth.ConfirmEmail, V1.Auth.ConfirmEmail)
            .RequireAuthorization();

        app.MapGet(Endpoints.V1.Auth.Profile, V1.Auth.Profile)
            .RequireAuthorization(Policies.RequireUserRole);

        app.MapGet(Endpoints.V1.Map.Search, V1.Map.Search)
            .RequireAuthorization();

        app.MapGet(Endpoints.V1.Map.Reverse, V1.Map.Reverse)
            .RequireAuthorization();

        app.MapGet(Endpoints.V1.Cities.Index, V1.Cities.GetAll)
            .RequireAuthorization();

        app.MapGet(Endpoints.V1.Categories.Index, V1.Categories.GetAll)
            .RequireAuthorization();

        app.MapGet(Endpoints.V1.Events.Index, V1.Events.GetByQuery) 
            .RequireAuthorization();

        app.MapPost(Endpoints.V1.Events.Index, V1.Events.Create)
            .RequireAuthorization();

        app.MapGet(Endpoints.V1.Events.Id.Index, V1.Events.GetById)
            .RequireAuthorization();

        app.MapPost(Endpoints.V1.Events.Id.Images, V1.Events.UploadImage)
            .RequireAuthorization();

        app.MapPost(Endpoints.V1.Events.Id.Likes, V1.Events.UpdateLikes)
            .RequireAuthorization();

        app.MapPost(Endpoints.V1.Events.Id.Comments, V1.Events.AddComment)
            .RequireAuthorization();

        app.MapGet(Endpoints.V1.Events.Id.Comments, V1.Events.GetComments)
            .RequireAuthorization();

        app.MapPost(Endpoints.V1.Requests.Index, V1.Requests.Create)
            .RequireAuthorization(Policies.RequireUserRole);

        app.MapGet(Endpoints.V1.Requests.Index, V1.Requests.GetAllByCityId)
            .RequireAuthorization();

        app.MapGet(Endpoints.V1.Requests.Types, V1.Requests.GetTypes)
            .RequireAuthorization();

        app.MapPost(Endpoints.V1.Requests.Id.Accept, V1.Requests.Accept)
            .RequireAuthorization(Policies.RequireAdminRole);

        app.MapPost(Endpoints.V1.Requests.Id.Reject, V1.Requests.Reject)
            .RequireAuthorization(Policies.RequireAdminRole);

        return app;
    }
}
