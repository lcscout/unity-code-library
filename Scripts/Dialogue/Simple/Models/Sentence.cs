using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A Sentence contains the sentence itself and the option to be a question (in this case with only two answers)
[System.Serializable]
public class Sentence {
	[Tooltip("The actual string of the sentence")]
	[TextArea(4, 12)]
	public string sentence;

	[Tooltip("If the sentence is a question or not")]
	public bool isQuestion;

	[Tooltip("The first answer - if the sentence is a question")]
	public Answer firstAnswer;

	[Tooltip("The second answer - if the sentence is a question")]
	public Answer secondAnswer;
}
