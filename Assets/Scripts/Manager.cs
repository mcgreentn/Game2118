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
    public Image Face;
    public Text DialogueText;


    public Animator DialogueAnimator;

    //public Queue<string> sentences;
    //public Queue<string> names;
    //public Queue<Sprite> faces;

    public Queue<Entity> entities;

    Coroutine Typing = null;

    public int interacting = 0;
    public int talking = 0;
    public Interactable currentInteractable;

	private void Start()
	{
        entities = new Queue<Entity>();
        //sentences = new Queue<string>();
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

    public void ShowDialoguePane() {
        //DialoguePane.SetActive(true);
        DialogueAnimator.SetBool("IsOpen", true);
        HideInteractionPane();
    }

    public void HideDialoguePane() {
        DialogueAnimator.SetBool("IsOpen", false);
        //DialoguePane.SetActive(false);
    }

    public void StartDialogue(Interactable me) {
        GameStats.CanMove = false;
        ShowDialoguePane();
        HideInteractionPane();
        StartCoroutine(TalkingToRoutine());
        Debug.Log("[Begin conversation with " + me.name + "]");

        entities.Clear();
        //sentences.Clear();
        //names.Clear();
        //faces.Clear();

        //foreach(string sentence in me.dialogues[me.counter].sentences) {
        //    sentences.Enqueue(sentence);
        //}
        foreach(Entity entity in me.dialogues[me.counter].entities){
            entities.Enqueue(entity);
        }
        me.counter++;
        if(me.counter > me.dialogues.Length-1) {
            me.counter = 0;
        }
        Debug.Log("Dialogue size: " + entities.Count);
        DisplayNextSentence();
        if(me.Type == 1) {
            // collect this object
            me.Collect();
        }
    }

    public void StartDialogue(Guard me) {
        GameStats.CanMove = false;
        ShowDialoguePane();
        HideInteractionPane();
        StartCoroutine(TalkingToRoutine());
        Debug.Log("[Begin conversation with " + me.name + "]");

        //sentences.Clear();
        entities.Clear();
        //foreach (string sentence in me.dialogue.sentences)
        //{
        //    sentences.Enqueue(sentence);
        //}

        foreach (Entity entity in me.dialogue.entities)
        {
            entities.Enqueue(entity);
        }
        Debug.Log("Dialogue size: " + entities.Count);
        DisplayNextSentence();

    }

    public void DisplayNextSentence() {
        if(entities.Count == 0) {
            EndDialogue();
            return;
        }

        Entity entity = entities.Dequeue();
        string sentence = entity.sentence;
        string name = entity.name;
        Sprite face = entity.image;
        DialogueName.text = name;
        Face.sprite = entity.image;
        //DialogueFace.sprite = face;
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
        char[] sent = sentence.ToCharArray();
        for (int i = 0; i < sentence.Length; i++) {
            DialogueText.text += sentence[i];
            if (i < sentence.Length - 1)
            {
                i++;
                DialogueText.text += sentence[i];
            }
            yield return null;
        }
        Typing = null;
    }
}
