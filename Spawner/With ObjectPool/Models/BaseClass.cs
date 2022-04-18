using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds an action for kill behaviour (to be released from Pool) and common methods
public abstract class BaseClass : MonoBehaviour {
	protected Action<BaseClass> _killAction;

	// Examples:
	protected abstract void OnCollisionEnter2D(Collision2D other);
	public virtual void ResetPosition() { }
	// -----------------------

	public void SetKill(Action<BaseClass> killAction) => _killAction = killAction;
}
