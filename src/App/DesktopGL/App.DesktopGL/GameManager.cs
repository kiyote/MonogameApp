using App.DesktopGL.Scenes.Main;
using Base.Core.StateMachines;

namespace App.DesktopGL;

internal class GameManager {

	public enum AppState {
		MainMenu,
		InGame
	}

	private readonly SimpleStateMachine<AppState> _state;

	private readonly MainScreen _mainScreen;

	public GameManager(
		MainScreen mainScreen
	) {
		_mainScreen = mainScreen;
		_state = new SimpleStateMachine<AppState>(
			new SimpleStateMachine<AppState>.Config(
				AppState.MainMenu,
				[
				],
				[
					new SimpleStateMachine<AppState>.StateMethods(
						AppState.MainMenu,
						() => { _mainScreen.Show(); },
						() => { _mainScreen.Hide(); }
					)
				]
			)
		);
	}

	public void Start() {
		_state.Start();
	}
}
