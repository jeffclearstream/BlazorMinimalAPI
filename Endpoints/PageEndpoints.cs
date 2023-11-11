using BlazorMinimalApis.Endpoints.Pages.Contacts;
using BlazorMinimalApis.Endpoints.Pages.Home;
using BlazorMinimalApis.Endpoints.Pages;

namespace BlazorMinimalApis.Endpoints;

public static class PageEndpoints
{
    public static WebApplication MapPageEndpoints(this WebApplication app)
    {
        app.MapGet("/", new HomeHandler().Index)
            .WithName("Home");

        app.MapGet("/contacts", new ContactHandler().List)
            .WithName("Contacts");

        app.MapGet("/contacts/search", new ContactHandler().Search)
            .WithName("Contacts.Search");

        app.MapGet("/contacts/create", new ContactHandler().Create)
            .WithName("Contacts.Create");

        app.MapPost("/contacts/create", new ContactHandler().Store)
            .WithName("Contacts.Store");

        app.MapGet("/contacts/{id:int}/edit", new ContactHandler().Edit)
            .WithName("Contacts.Edit");

        app.MapPost("/contacts/{id:int}/edit", new ContactHandler().Update)
            .WithName("Contacts.Update");

        app.MapGet("/contacts/{id:int}/delete", new ContactHandler().Delete)
            .WithName("Contacts.Delete");

        app.MapGet("dataset/{id:int}", new DatasetHandler().Edit)
            .WithName("Dataset.View");

        app.MapGet("/login", new DatasetHandler().Login);
        app.MapGet("/signin", new DatasetHandler().SignIn).WithName("Signin");
        app.MapGet("/register", new DatasetHandler().Register);

        return app;
    }
}