using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour {

    /// <summary>
    /// The interaction pane.
    /// </summary>
    public GameObject InteractionPane;
    public Text InteractionName;
    public Text InteractionText;


    public GameObject DialoguePane;
    public Text DialogueName;
    public SpriteRenderer Face;
    public Text DialogueText;


    public Animator DialogueAnimator;

    public Queue<string> sentences;

    Coroutine Typing = null;

    public int interacting = 0;
    public int talking = 0;
    public Interactable currentInteractable;

	private void Start()
	{
        sentences = new Queue<string>();
	}
	void Update()
	{
        if(Input.GetKeyDown(KeyCode.Space) && interacting == 1) 
        {
            StartDialogue(currentInteractable);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && talking == 1)
        {
            DisplayNextSentence();
        }
	}
    public void ShowInteractionPane(string name, string text, Interactable me) {
        currentInteractable = me;
        InteractionPane.SetActive(true);
        SetUpInteractionPane(name, text);
        interacting = 1;
    }

    public void SetUpInteractionPane(string name, string text) {
        InteractionName.text = name;
        InteractionText.text = text;
    }

    public void HideInteractionPane() {
        currentInteractable = null;
        InteractionPane.SetActive(false);
        interacting = 0;
    }

    public void ShowDialoguePane(string name, Sprite image) {
        //DialoguePane.SetActive(true);
        DialogueAnimator.SetBool("IsOpen", true);
        DialogueName.text = name;
        Face.sprite = image;
        HideInteractionPane();
    }

    public void HideDialoguePane() {
        DialogueAnimator.SetBool("IsOpen", false);
        //DialoguePane.SetActive(false);
    }

    public void StartDialogue(Interactable me) {
        GameStats.CanMove = false;
        ShowDialoguePane(me.name, me.image);
        HideInteractionPane();
        StartCoroutine(TalkingToRoutine());
        Debug.Log("[Begin conversation with " + me.name + "]");

        sentences.Clear();

        foreach(string sentence in me.dialogues[me.counter].sentences) {
            sentences.Enqueue(sentence);
        }
        me.counter++;
        if(me.counter > me.dialogues.Length-1) {
            me.counter = 0;
        }
        Debug.Log("Sentences size: " + sentences.Count);
        DisplayNextSentence();
    }

    public void StartDialogue(Guard me) {
        GameStats.CanMove = false;
        ShowDialoguePane(me.dialogue.name, me.image);
        HideInteractionPane();
        StartCoroutine(TalkingToRoutine());
        Debug.Log("[Begin conversation with " + me.dialogue.name + "]");

        sentences.Clear();

        foreach (string sentence in me.dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        Debug.Log("Sentences size: " + sentences.Count);
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //DialogueText.text = sentence;
        if(Typing != null) {
            StopCoroutine(Typing);
        }
        Typing = StartCoroutine(TypeSentence(sentence));
        Debug.Log(sentence);
    }

    void EndDialogue() {
        GameStats.CanMove = true;
        talking = 0;
        currentInteractable = null;
        HideDialoguePane();
        Debug.Log("[Ending dialogue]");
    }


    IEnumerator TalkingToRoutine() {
        yield return null;
        talking = 1;
    }

    IEnumerator TypeSentence(string sentence) {
        DialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            DialogueText.text += letter;
            yield return null;
        }
        Typing = null;
    }
}
