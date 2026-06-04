using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace App.DesktopGL;

internal class Program {
	private static void Main(
		string[] args
	) {
		ConfigurationManager configuration = BuildConfiguration( args );

		DisplayOptions opts = new DisplayOptions();
		configuration.GetSection( DisplayOptions.Section ).Bind( opts );

		MonoManager game = new MonoManager( configuration );
		game.Run();
	}

	private static ConfigurationManager BuildConfiguration(
		string[] args
	) {
		var configuration = new ConfigurationManager();

		string environment = Environment.GetEnvironmentVariable( "DOTNET_ENVIRONMENT" )
							 ?? Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" )
							 ?? Environments.Production;

		configuration
			.AddJsonFile( "appsettings.json", optional: true, reloadOnChange: true )
			.AddJsonFile( $"appsettings.{environment}.json", optional: true, reloadOnChange: true )
			.AddEnvironmentVariables()
			.AddCommandLine( args );

		return configuration;
	}
}
