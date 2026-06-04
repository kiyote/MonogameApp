using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;

namespace App.DesktopGL.Scenes.Main;

internal sealed class MainScreen : Screen {

	private readonly SpriteFont _font;
	private readonly IGameTerminator _terminator;
	private readonly SpriteBatch _spriteBatch;

	public MainScreen(
		SpriteBatch spriteBatch,
		ContentManager content,
		IGameTerminator terminator
	) {
		_terminator = terminator;
		_spriteBatch = spriteBatch;
		_font = content.Load<SpriteFont>( "UI" );
	}

	public override void Draw(
		GameTime gameTime
	) {
		_spriteBatch.Begin( SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp );
		_spriteBatch.DrawString( _font, "Hello world!", new Vector2( 100.0f, 100.0f ), Color.White );
		_spriteBatch.End();
	}

	public override void Update(
		GameTime gameTime
	) {
		if( Keyboard.GetState().IsKeyDown( Keys.Escape ) ) {
			_terminator.Exit();
		}
	}
}
