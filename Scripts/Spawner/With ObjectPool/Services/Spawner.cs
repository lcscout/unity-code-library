using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// The spawner handles an ObjectPool (you could implement a version with more than one) to get the object when needed and release it back when not.
public class Spawner : MonoBehaviour {
	[Tooltip("The BaseClass prefab")]
	[SerializeField] private BaseClass _baseClassPrefab;

	[Tooltip("A delay time in seconds to wait before start spawning")]
	[SerializeField] private float _startDelay = 2;

	[Tooltip("The time in seconds in which the spawn will keep repeating")]
	[SerializeField] private float _repeatRate = 2;

	private ObjectPool<BaseClass> _pool;
	private float _timer = 0f;

	private void Start() {
		_pool = new ObjectPool<BaseClass>(CreateBaseClass, OnGetBaseClassFromPool, OnReleaseBaseClassToPool, BaseClass => Destroy(BaseClass.gameObject), true, 6, 12);
		_timer = _startDelay;
	}

	private void Update() {
		if (/* condition is */ true) {
			timer -= Time.deltaTime;
			if (timer < 0) {
				timer = repeatRate;
				SpawnBaseClass();
			}
		}
	}

	// Defines the method that will be called when the pool needs to get an object but there isn't one available, therefore it creates one.
	private BaseClass CreateBaseClass() {
		return Instantiate(_baseClassPrefab, transform.position, _baseClassPrefab.transform.rotation);
	}

	// Method called when the object is 'getted', activating gameObject is essential
	private void OnGetBaseClassFromPool(BaseClass BaseClass) {
		BaseClass.gameObject.SetActive(true);
		BaseClass.ResetPosition();
	}

	// Method called when the object is released, deactivating gameObject is essential
	private void OnReleaseBaseClassToPool(BaseClass BaseClass) => BaseClass.gameObject.SetActive(false);

	// On spawn the object is simply activated (get) and their killAction is setted
	private void SpawnBaseClass() {
		BaseClass BaseClass = _pool.Get();
		BaseClass.GetComponent<BaseClass>().SetKill(Kill);
	}

	// The killAction is simply a release from the pool, this way we have a reference for the pool inside the BaseClass/InheritedClass
	private void Kill(BaseClass BaseClass) => _pool.Release(BaseClass);
}
