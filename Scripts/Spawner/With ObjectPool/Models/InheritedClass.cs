using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Each inherited class implements their spefics behaviours as well as the call for killAction
public class InheritedClass : BaseClass {

	// Example of killAction call
	protected override void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Player")) {
			_killAction(this);
			GameManager.Instance.GameOver();
		}
	}
	// -----------------------

	// Example of specific behaviour
	private void Start() => AssignNewVelocity();
	private void AssignNewVelocity() => GetComponent<Rigidbody2D>().velocity = Random.Range(_minVelocity, _maxVelocity) * Time.fixedDeltaTime;
	// -----------------------
}
