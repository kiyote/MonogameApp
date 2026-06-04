using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.DesktopGL.Scenes.Main;

public static class ExtensionMethods {

	public static IServiceCollection AddMainScreen(
		this IServiceCollection services
	) {
		services.TryAddSingleton<MainScreen>();
		return services;
	}
}
