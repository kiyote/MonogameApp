using App.DesktopGL.Scenes.Main;
using Microsoft.Extensions.DependencyInjection;

namespace App.DesktopGL.Scenes;

public static class ExtensionMethods {

	public static IServiceCollection AddScenes(
		this IServiceCollection services
	) {
		services.AddMainScreen();

		return services;
	}
}
