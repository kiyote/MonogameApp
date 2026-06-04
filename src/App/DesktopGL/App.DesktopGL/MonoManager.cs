using System;
using System.Collections.Generic;
using System.Globalization;
using App.DesktopGL.Localization;
using App.DesktopGL.Scenes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

namespace App.DesktopGL;

internal class MonoManager : Game, IGameTerminator {
	private readonly GraphicsDeviceManager _graphicsDeviceManager;
	private readonly ScreenManager _screenManager;
	private readonly IConfiguration _configuration;
	private ServiceProvider _services = default!;

	public MonoManager(
		IConfiguration configuration
	) {
		_configuration = configuration;		

		DisplayOptions opts = new DisplayOptions();
		configuration.GetSection( DisplayOptions.Section ).Bind( opts );

		_graphicsDeviceManager = new GraphicsDeviceManager( this ) {
			PreferredBackBufferWidth = opts.Width,
			PreferredBackBufferHeight = opts.Height,
			SynchronizeWithVerticalRetrace = opts.VSync,
			PreferredBackBufferFormat = SurfaceFormat.Color,
		};

		_screenManager = new ScreenManager();
		Components.Add( _screenManager );
		Content.RootDirectory = "Content";
	}

	protected override void LoadContent() {
		base.LoadContent();
		IServiceCollection services = new ServiceCollection();
		services.AddSingleton( GraphicsDevice );
		services.AddSingleton( Content );
		services.AddSingleton<IGameTerminator>( this );
		services.AddSingleton( _configuration );
		services.AddSingleton( _screenManager );
		services.AddSingleton<SpriteBatch>();
		services.AddSingleton<GameManager>();
		services.Configure<DisplayOptions>( _configuration.GetSection( DisplayOptions.Section ) );
		services.AddScenes();
		_services = services.BuildServiceProvider();
	}

	protected override void Initialize() {
		Window.AllowUserResizing = true;
		Window.ClientSizeChanged += OnWindowResize;
		IsMouseVisible = true;

		base.Initialize();

		// Load supported languages and set the default language.
		List<CultureInfo> cultures = LocalizationManager.GetSupportedCultures();
		var languages = new List<CultureInfo>();
		for( int i = 0; i < cultures.Count; i++ ) {
			languages.Add( cultures[i] );
		}

		// TODO You should load this from a settings file or similar,
		// based on what the user or operating system selected.
		string selectedLanguage = LocalizationManager.DEFAULT_CULTURE_CODE;
		LocalizationManager.SetCulture( selectedLanguage );

		GameManager app = _services.GetRequiredService<GameManager>();
		app.Start();
	}

	protected override void Draw(
		GameTime gameTime
	) {
		GraphicsDevice.Clear( Color.Black );
		base.Draw( gameTime );
	}

	private void OnWindowResize(
		object? sender,
		EventArgs e
	) {
		Window.ClientSizeChanged -= OnWindowResize;

		int minWidth = 800;
		int minHeight = 600;

		int targetWidth = Window.ClientBounds.Width;
		int targetHeight = Window.ClientBounds.Height;

		if( targetWidth < minWidth
			|| targetHeight < minHeight
		) {
			_graphicsDeviceManager.PreferredBackBufferWidth = targetWidth;
			_graphicsDeviceManager.PreferredBackBufferHeight = targetHeight;
			_graphicsDeviceManager.ApplyChanges();
		}

		Window.ClientSizeChanged += OnWindowResize;
	}
}

