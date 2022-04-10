using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

// Simple Dialogue with a two answers system, it verifies if the current sentence is a question and show the two possible answers
public class DialogueManager : MonoBehaviour {
	[SerializeField] private Queue<Sentence> _sentences = new Queue<Sentence>();
	[SerializeField] private TMP_Text _nameText;
	[SerializeField] private TMP_Text _conversationText;
	[SerializeField] private GameObject _answersGroup;
	[SerializeField] private TMP_Text _firstAnswerText;
	[SerializeField] private TMP_Text _secondAnswerText;
	[SerializeField] private GameObject _selectionPointer;

	private Sentence _currentSentence;
	private string _selectedAnswer = "first";
	private bool _canDisplayNextSentence = true; // used in conjuction to an async method to type the sentences letter by letter

	private void Update() {
		if (!_currentSentence.isQuestion && /* interaction to continue, like: */ Input.GetKeyDown(KeyCode.E) && _canDisplayNextSentence)
			DisplayNextSentence();
		else if (_currentSentence.isQuestion && _canDisplayNextSentence)
			HandleAnswerSelection();
	}

	// The method that starts the Dialogue (can be called from an interation with a npc for example)
	public void StartDialogue(Dialogue dialogue) {
		_nameText = dialogue.name;
		EnqueueSentences(dialogue);
		DisplayNextSentence();
	}

	private void EnqueueSentences(Sentence[] sentences) {
		_sentences.Clear();

		foreach (Sentence sentence in sentences)
			_sentences.Enqueue(sentence);
	}

	// Method to display next sentence enqueued
	private void DisplayNextSentence() {
		if (_sentences.Count == 0)
			return;

		ChangeAnswersGroupVisibility(false);

		_currentSentence = _sentences.Dequeue();
		TypeSentence(_conversationText, _currentSentence.sentence, true);
	}

	private void ChangeAnswersGroupVisibility(bool visibility) => _answersGroup.SetActive(visibility);

	// Method that types the sentence letter by letter by using async/await and Tasks. The typing speed could be configurable
	private async void TypeSentence(TMP_Text text, string sentence, bool checkQuestion = false) {
		_canDisplayNextSentence = false;

		text.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			text.text += letter;
			await Task.Yield();
		}

		if (checkQuestion)
			CheckIfQuestion();

		_canDisplayNextSentence = true;
	}

	private async void CheckIfQuestion() {
		if (_currentSentence.isQuestion) {
			ChangeAnswersGroupVisibility(true);
			PopulateAnswers(_currentSentence);
		}

		await Task.Yield();
	}

	private void PopulateAnswers(Sentence sentence) {
		TypeSentence(_firstAnswerText, sentence.firstAnswer.answer);
		TypeSentence(_secondAnswerText, sentence.secondAnswer.answer);
	}

	private void HandleAnswerSelection() {
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			SelectFirstAnswer();
		if (Input.GetKeyDown(KeyCode.RightArrow))
			SelectSecondAnswer();
		if (Input.GetKeyDown(KeyCode.E))
			CheckIfCorrectAnswer();
	}

	private void SelectFirstAnswer() {
		// The selecting can be shown by having a pointer and changing it's position, like:

		_selectionPointer.transform.SetParent(_firstAnswerText.transform);
		_selectionPointer.transform.localPosition = Vector3.left * (Mathf.Abs(_firstAnswerText.transform.localPosition.x) - 40);

		// --------------------------------------------------------------------------------

		_selectedAnswer = "first";
	}

	private void SelectSecondAnswer() {
		// _selectionPointer.transform.SetParent(_secondAnswerText.transform);
		// _selectionPointer.transform.localPosition = Vector3.left * (Mathf.Abs(_secondAnswerText.transform.localPosition.x) - 40);

		_selectedAnswer = "second";
	}

	// Method that checks if the correct answers was selected, by checking the Answer model
	private void CheckIfCorrectAnswer() {
		if ((_selectedAnswer == "first" && _currentSentence.firstAnswer.isCorrectAnswer) ||
			(_selectedAnswer == "second" && _currentSentence.secondAnswer.isCorrectAnswer))
			HandleCorrect();
		else if ((_selectedAnswer == "first" && !_currentSentence.firstAnswer.isCorrectAnswer) ||
			(_selectedAnswer == "second" && !_currentSentence.secondAnswer.isCorrectAnswer))
			HandleWrong();
	}

	private void HandleCorrect() {
		CleanDialogue();
		// GameManager.Instance.Continue();
	}

	private void HandleWrong() {
		CleanDialogue();
		// GameManager.Instance.GameOver();
	}

	// Method to clean the dialogue back to nothing
	private void CleanDialogue() {
		_nameText.text = "";
		_conversationText.text = "";
		ChangeAnswersGroupVisibility(false);
	}
}
