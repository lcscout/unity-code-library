using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Answer {
	[Tooltip("If this is the correct answer for question or not")]
	public bool isCorrectAnswer;

	[Tooltip("The actual string of the answer")]
	public string answer;
}
