using System;
using System.Collections.Generic;

// The state machine works by holding a dictionary that links a list of transitions by state. This way the FSM can change states whenever the conditions are met
public class StateMachine {
	private class Transition {
		public Func<bool> Condition { get; }
		public IState To { get; }

		public Transition(IState to, Func<bool> condition) {
			To = to;
			Condition = condition;
		}
	}

	private IState _currentState;

	private Dictionary<Type, List<Transition>> _transitionsListByType = new Dictionary<Type, List<Transition>>();
	private List<Transition> _currentTransitionsList = new List<Transition>();
	private List<Transition> _anyTransitionsList = new List<Transition>();

	private readonly List<Transition> EmptyTransitions = new List<Transition>(0);

	// Main method - verifies the state situation and calls said state "action" method
	public void Tick() {
		Transition transition = GetTransition();
		if (transition != null)
			SetState(transition.To);

		_currentState?.Tick();
	}

	// Method that change states, calling OnExit method of the state before changing and OnEnter after
	public void SetState(IState state) {
		if (state == _currentState)
			return;

		_currentState?.OnExit();
		_currentState = state;

		// Try to get list of transitions of _currentState
		_transitionsListByType.TryGetValue(_currentState.GetType(), out _currentTransitionsList);
		if (_currentTransitionsList == null)
			_currentTransitionsList = EmptyTransitions;

		_currentState.OnEnter();
	}

	// Used to set up a normal transition, these have two conditions.
	// One being a true condition, a func predicate that returns a bool, and the other being the previous state that the AI has to be
	public void AddTransition(IState from, IState to, Func<bool> condition) {
		// if the dictionary entry doesn't exist, create one
		if (_transitionsListByType.TryGetValue(from.GetType(), out List<Transition> transitions) == false) {
			transitions = new List<Transition>();
			_transitionsListByType[from.GetType()] = transitions;
		}

		// because of the 'out' parameter we can add to dictionary entry either way
		transitions.Add(new Transition(to, condition));
	}

	// Used to set up a 'any' transition, these are called before every other and don't depend of a previous state, only a predicate
	public void AddAnyTransition(IState to, Func<bool> condition) {
		_anyTransitions.Add(new Transition(to, condition));
	}

	// The method that returns the transitions if their conditions are met, notice _anyTransitions are called first
	private Transition GetTransition() {
		foreach (Transition transition in _anyTransitions)
			if (transition.Condition())
				return transition;

		foreach (Transition transition in _currentTransitionsList)
			if (transition.Condition())
				return transition;

		return null;
	}
}