using System;
using System.Collections.Generic;
using System.Globalization;
using App.DesktopGL.Localization;
using App.DesktopGL.Scenes;
using Gum.Forms.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;

namespace App.DesktopGL;

internal class MonoManager : Game, IGameTerminator {
	private readonly GraphicsDeviceManager _graphicsDeviceManager;
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

		Content.RootDirectory = "Content";
	}

	protected override void LoadContent() {
		base.LoadContent();
		IServiceCollection services = new ServiceCollection();
		services.AddSingleton( GraphicsDevice );
		services.AddSingleton( Content );
		services.AddSingleton<Game>( this );
		services.AddSingleton<IGameTerminator>( this );
		services.AddSingleton( _configuration );
		services.AddSingleton<GameManager>();
		services.Configure<DisplayOptions>( _configuration.GetSection( DisplayOptions.Section ) );
		services.AddScenes();
		_services = services.BuildServiceProvider();
	}

	protected override void Initialize() {
		Window.AllowUserResizing = true;
		Window.ClientSizeChanged += OnWindowResize;
		IsMouseVisible = true;
		GumService.Default.Initialize( this );
		GumService.Default.EnableExpandToWindow();
		FrameworkElement.KeyboardsForUiControl.Add( GumService.Default.Keyboard );

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

	protected override void Update(
		GameTime gameTime
	) {
		GumService.Default.Update( gameTime );
		base.Update( gameTime );
	}

	protected override void Draw(
		GameTime gameTime
	) {
		GraphicsDevice.Clear( Color.DarkSlateBlue );
		GumService.Default.Draw();
		base.Draw( gameTime );
	}

	private void OnWindowResize(
		object? sender,
		EventArgs e
	) {
		Window.ClientSizeChanged -= OnWindowResize;

		int targetWidth = Math.Max( 960, Window.ClientBounds.Width );
		int targetHeight = Math.Max( 540, Window.ClientBounds.Height );

		_graphicsDeviceManager.PreferredBackBufferWidth = targetWidth;
		_graphicsDeviceManager.PreferredBackBufferHeight = targetHeight;
		_graphicsDeviceManager.ApplyChanges();

		Window.ClientSizeChanged += OnWindowResize;
	}
}

