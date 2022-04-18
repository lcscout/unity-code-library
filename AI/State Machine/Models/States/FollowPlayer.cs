using UnityEngine;
using UnityEngine.AI;

// An example state for following the player
public class FollowPlayer : IState {
	private readonly EnemyController _enemy;
	private readonly float _minRadiusFromPlayer;

	private NavMeshAgent _navMeshAgent;

	public FollowPlayer(EnemyController enemy, NavMeshAgent navMeshAgent, float minRadiusFromPlayer) {
		_enemy = enemy;
		_minRadiusFromPlayer = minRadiusFromPlayer;
		_navMeshAgent = navMeshAgent;
	}
	public void Tick() {
		FacePlayer();
		MoveToPlayer();
	}

	public void OnEnter() => _navMeshAgent.enabled = true;
	public void OnExit() => _navMeshAgent.enabled = false;

	private void MoveToPlayer() {
		Vector3 direction = (Player.Instance.transform.position - _enemy.transform.position).normalized;
		_navMeshAgent.SetDestination(Player.Instance.transform.position - (direction * _minRadiusFromPlayer));
	}

	private void FacePlayer() {
		Vector3 direction = (Player.Instance.transform.position - _enemy.transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		_enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, lookRotation, Time.deltaTime * 5f);
	}
}