using Microsoft.AspNetCore.Builder;

namespace BlazorMinimalApis.Pages.Lib;

public static class RegisterRoutesExtension
{
	public static void RegisterRoutes(this WebApplication app)
	{
	
		var endpointDefinitions = typeof(Program).Assembly
			.GetTypes()
			.Where(t => t.IsAssignableTo(typeof(IRouteDefinition))
			            && !t.IsAbstract && !t.IsInterface)
			.Select(t =>
			{
                using var scope = app.Services.CreateScope();
                var result = scope.ServiceProvider.GetService(t);
				return result == null ? Activator.CreateInstance(t) : result;
			})
			.Cast<IRouteDefinition>();

		foreach (var endpointDef in endpointDefinitions)
		{
			endpointDef.Map(app);
		}
	}
}