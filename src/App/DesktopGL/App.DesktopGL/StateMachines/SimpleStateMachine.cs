using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Core.StateMachines;

internal sealed class SimpleStateMachine<T> where T : struct, Enum {

	public record StateMethods(
		T TargetState,
		Action OnEnter,
		Action OnLeave
	);

	public record Transition(
		T From,
		T To
	);

	public record Config(
		T InitialState,
		IEnumerable<Transition> Transitions,
		IEnumerable<StateMethods> Methods
	);

	private T _currentState;
	private readonly Transition[] _transitions;
	private readonly StateMethods[] _methods;

	public SimpleStateMachine(
		Config config
	) {
		_currentState = default;

		_transitions = [ ..config.Transitions ];
		_methods = [.. config.Methods];
	}

	public T State => _currentState;

	public void Go(
		T to
	) {
		Transition transition = _transitions
			.FirstOrDefault( t => t.From.Equals( _currentState ) && t.To.Equals( to ) )
				?? throw new InvalidOperationException( $"No transition exists from state {_currentState} to state {to}." );

		StateMethods? fromMethods = _methods.FirstOrDefault( m => m.TargetState.Equals( _currentState ) );
		StateMethods? toMethods = _methods.FirstOrDefault( m => m.TargetState.Equals( to ) );

		fromMethods?.OnLeave.Invoke();
		_currentState = to;
		toMethods?.OnEnter.Invoke();
	}

	public void Start() {
		StateMethods? initialMethods = _methods.FirstOrDefault( m => m.TargetState.Equals( _currentState ) );
		initialMethods?.OnEnter.Invoke();
	}
}
