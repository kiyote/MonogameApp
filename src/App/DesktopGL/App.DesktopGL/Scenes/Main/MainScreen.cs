using Gum.Converters;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.GueDeriving;
using Gum.Wireframe;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RenderingLibrary.Graphics;

namespace App.DesktopGL.Scenes.Main;

internal sealed class MainScreen {

	private readonly IGameTerminator _terminator;
	private readonly Panel _root;

	public MainScreen(
		ContentManager content,
		IGameTerminator terminator
	) {
		_terminator = terminator;

		_root = new Panel() {
			X = 50,
			Y = 50,
			Width = 300,
			Height = 400,
			XUnits = GeneralUnitType.Percentage,
			YUnits = GeneralUnitType.Percentage,
			WidthUnits = DimensionUnitType.Absolute,
			HeightUnits = DimensionUnitType.Absolute,
			XOrigin = HorizontalAlignment.Center,
			YOrigin = VerticalAlignment.Center
		};

		Texture2D panelTexture = content.Load<Texture2D>( "Panel" );
		NineSliceRuntime panel = new NineSliceRuntime {
			Texture = panelTexture,
		};
		panel.Dock( Dock.Fill );
		_root.AddChild( panel );

		Button button = new Button {
			XOrigin = HorizontalAlignment.Right,
			YOrigin = VerticalAlignment.Bottom,
			XUnits = GeneralUnitType.PixelsFromLarge,
			YUnits = GeneralUnitType.PixelsFromLarge,
			Text = "Quit to desktop",
			X = -20,
			Y = -20,
			Width = 200
		};
		button.Click += ( _, __ ) => { _terminator.Exit(); };

		_root.AddChild( button );

	}

	public void Show() {
		_root.AddToRoot();
	}

	public void Hide() {
		_root.RemoveFromRoot();
	}

}
