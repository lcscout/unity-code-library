using System;
using UnityEngine;
using UnityEngine.AI;

// Example of an AI entity using state machine
[RequireComponent(typeof(NavMeshAgent))]
public class EntityController : MonoBehaviour {
	[SerializeField] private float _lookRadius = 10f;
	[SerializeField] private float _minDistanceRadius = 1f;

    private StateMachine _stateMachine;
	private NavMeshAgent _navMeshAgent;

	private void Awake() {
		_navMeshAgent = GetComponent<NavMeshAgent>();

		// State machine setup

        _stateMachine = new StateMachine();

		// Create states
        IState roam = new Roam(this, _navMeshAgent);
        IState followPlayer = new FollowPlayer(this, _navMeshAgent, _minDistanceRadius);

		// Setup transitions conditions
		Func<bool> PlayerInRange() => () => Vector3.Distance(Player.Instance.transform.position, transform.position) <= _lookRadius;
		Func<bool> PlayerNotInRange() => () => !(Vector3.Distance(Player.Instance.transform.position, transform.position) <= _lookRadius);

		// Short call for adding transitions
		void At(IState from, Func<bool> condition, IState to) => _stateMachine.AddTransition(from, to, condition);

		// Effectively add transitions
		At(roam, PlayerInRange(), followPlayer);
		At(followPlayer, PlayerNotInRange(), roam);

		// Set initial state
		_stateMachine.SetState(roam);

		// -------------------
	}

	// State machine Tick is called every frame
	private void Update() => _stateMachine.Tick();

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _lookRadius);
		Gizmos.DrawWireSphere(transform.position, _minDistanceRadius);
	}
}
