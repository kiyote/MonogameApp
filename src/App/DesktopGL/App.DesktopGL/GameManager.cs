using App.DesktopGL.Scenes.Main;
using Base.Core.StateMachines;
using MonoGame.Extended.Screens;

namespace App.DesktopGL;

internal class GameManager {

	public enum AppState {
		MainMenu,
		InGame
	}

	private readonly SimpleStateMachine<AppState> _state;
	private readonly ScreenManager _screenManager;
	private readonly MainScreen _mainScreen;

	public GameManager(
		ScreenManager screenManager,
		MainScreen mainScreen
	) {
		_screenManager = screenManager;
		_mainScreen = mainScreen;
		_state = new SimpleStateMachine<AppState>(
			new SimpleStateMachine<AppState>.Config(
				AppState.MainMenu,
				[
				],
				[
					new SimpleStateMachine<AppState>.StateMethods(
						AppState.MainMenu,
						() => _screenManager.ReplaceScreen( _mainScreen ),
						() => {}
					)
				]
			)
		);
	}

	public void Start() {
		_state.Start();
	}
}
