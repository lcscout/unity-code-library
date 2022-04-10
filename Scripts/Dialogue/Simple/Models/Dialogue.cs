using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This model holds the definition of a dialogue, in this simple case this are the name of the npc and it's sentences
[System.Serializable]
public class Dialogue {
	[Tooltip("The name of the NPC that is talking")]
	public string name;

	[Tooltip("An array of the sentences of the dialogue")]
	public Sentence[] sentences;
}
