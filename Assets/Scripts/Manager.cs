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

    // The player character
    public GameObject Player;

    public ParticleSystem WhereToGo;
    //public Queue<string> sentences;
    //public Queue<string> names;
    //public Queue<Sprite> faces;

    public Queue<Entity> entities;

    Coroutine Typing = null;

    public int interacting = 0;
    public int talking = 0;
    public Interactable currentInteractable;


    public int eventNum = 0;
    // Fade Animator
    public Animator FadeAnimator;


    public Vector2 PlayerRespawn;

    //The Bad Guy
    public GameObject TheBadMan;
    public GameObject DoorGuard;
    public GameObject ExtraGuard;

	private void Start()
	{
        entities = new Queue<Entity>();
        //sentences = new Queue<string>();
	}
	void Update()
	{
        if(Input.GetKeyDown(KeyCode.Space) && interacting == 1) 
        {
            if (GameStats.IsStealthed && currentInteractable.Type != 1)
            {
                StartEvesdropping(currentInteractable);
            }
            else
            {
                StartDialogue(currentInteractable);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && talking == 1)
        {
            DisplayNextSentence();
        }else if(Input.GetKeyDown(KeyCode.F) && interacting == 1) {
            StartInvestigate(currentInteractable);
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

        foreach(Entity entity in me.dialogues[me.counter].entities){
            entities.Enqueue(entity);
        }
        me.counter++;
        if(me.counter > me.dialogues.Length-1) {
            me.counter = 0;
        }
        Debug.Log("Dialogue size: " + entities.Count);
        DisplayNextSentence();
        if(me.Type == 2) {
            // collect this object
            me.Collect();
        }
    }

    public void StartEvesdropping(Interactable me)
    {
        GameStats.CanMove = false;
        ShowDialoguePane();
        HideInteractionPane();
        StartCoroutine(TalkingToRoutine());
        Debug.Log("[Begin conversation with " + me.name + "]");

        entities.Clear();

        if (me.evesdroppings.Length > 0)
        {
            foreach (Entity entity in me.evesdroppings[0].entities)
            {
                entities.Enqueue(entity);
            }
        }
        Debug.Log("Dialogue size: " + entities.Count);
        DisplayNextSentence();
        if (me.Type == 2)
        {
            // collect this object
            me.Collect();
        }
    }

    public void StartInvestigate(Interactable me) {
        GameStats.CanMove = false;
        ShowDialoguePane();
        HideInteractionPane();
        StartCoroutine(TalkingToRoutine());
        Debug.Log("[Begin investigate with " + me.name + "]");

        foreach (Entity entity in me.investigates[0].entities)
        {
            entities.Enqueue(entity);
        }

        Debug.Log("Investigates size: " + entities.Count);
        DisplayNextSentence();
        if (me.Type == 1)
        {
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

        entities.Clear();

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
        eventNum = entity.eventNum;
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
        if (eventNum != 0)
        {
            PlayEvent(eventNum);
        }
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

    public void Level1_0() {
        PlayerRespawn = new Vector2(-3.5f, -7.5f);
    }

    public void Level1_1() {
        WhereToGo.gameObject.transform.localPosition = new Vector2(175f, 292f);
        WhereToGo.Play();
    }

    public void Level1_2() {
        WhereToGo.gameObject.transform.localPosition = new Vector2(-176f, -294f);
        WhereToGo.Play();
    }
    public void Level1_3() {
        WhereToGo.gameObject.transform.localPosition = new Vector2(-178f, -289f);
        WhereToGo.Play();
        // fade to black briefly
        FadeAnimator.SetBool("Fade", true);
        StartCoroutine(MakeBadGuyLeave1());
        PlayerRespawn = new Vector2(-1.0f, -1.0f);
    }

    public void ResetLevel1() {
        FadeAnimator.SetBool("Fade", true);
        StartCoroutine(Reset1());
    }

    public void GoToLevel2() {
        // TODO make level 2
    }
    public void PlayEvent(int eventNum) {
        if(eventNum == 1) {
            Level1_1();
        }
        else if(eventNum == 2) {
            Level1_2();
        }
        else if(eventNum == 3) {
            Level1_3();
        }
        else if(eventNum == 4) {
            GoToLevel2();
        }
        else if(eventNum == 90) {
            ResetLevel1();
        }
    }





    IEnumerator Reset1() {
        yield return new WaitForSeconds(1.0f);
        Player.transform.position = PlayerRespawn;
            //new Vector2(-3.5f, -7.5f);
        FadeAnimator.SetBool("Fade", false);
    }
    IEnumerator MakeBadGuyLeave1() {
        yield return new WaitForSeconds(1.0f);
        TheBadMan.SetActive(false);
        DoorGuard.SetActive(false);
        ExtraGuard.SetActive(true);
        FadeAnimator.SetBool("Fade", false);

    }
}
