using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
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

    public GameObject HeadOfSecurity;
    public GameObject Guards;
    public GameObject Jenkins;

    // Security Door
    public Animator SecurityDoor;

    // Maps
    public GameObject Level1;
    public GameObject Level2;
    public GameObject Level3_1;
    public GameObject Level3_2;
    public GameObject Level4;



    public GameObject Theater;
	private void Start()
	{
        entities = new Queue<Entity>();
        //PlayMovie();
        Level1_0();
        //GoToLevel2();
        //GoToLevel3_1From2();
        //GoToLevel4();
        //sentences = new Queue<string>();
	}
	void Update()
	{
        if(Input.GetKeyDown(KeyCode.Space) && interacting == 1) 
        {
            if (GameStats.IsStealthed && (currentInteractable.Type < 1))
            {
                eventNum = 0;
                StartEvesdropping(currentInteractable);

            } 
            else
            {
                eventNum = 0;
                specialInstanceCheck();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && talking == 1)
        {
            DisplayNextSentence();
        }else if(Input.GetKeyDown(KeyCode.F) && interacting == 1) {
            eventNum = 0;
            StartInvestigate(currentInteractable);
        }
	}

    public void PlayMovie() {
        Theater.SetActive(true);
        PlayMyClip();
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
        talking = 1;
        interacting = 0;
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

    public void StartDialogue(Interactable me, int count)
    {
        GameStats.CanMove = false;
        ShowDialoguePane();
        HideInteractionPane();
        talking = 1;
        interacting = 0;
        //StartCoroutine(TalkingToRoutine());
        Debug.Log("[Begin conversation with " + me.name + "]");

        entities.Clear();

        foreach (Entity entity in me.dialogues[count].entities)
        {
            entities.Enqueue(entity);
        }

        Debug.Log("Dialogue size: " + entities.Count);
        DisplayNextSentence();
        if (me.Type == 2)
        {
            // collect this object
            me.Collect();
        }
    }

    public void StartEvesdropping(Interactable me)
    {
        GameStats.CanMove = false;
        ShowDialoguePane();
        HideInteractionPane();
        //StartCoroutine(TalkingToRoutine());
        talking = 1;
        interacting = 0;
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
        //StartCoroutine(TalkingToRoutine());
        talking = 1;
        interacting = 0;
        Debug.Log("[Begin investigate with " + me.name + "]");

        foreach (Entity entity in me.investigates[0].entities)
        {
            entities.Enqueue(entity);
        }

        Debug.Log("Investigates size: " + entities.Count);
        DisplayNextSentence();
        if (me.Type == 2)
        {
            // collect this object
            me.Collect();
        }

    }

    public void Announcement(int id) {
        GameStats.CanMove = false;
        ShowDialoguePane();
        HideInteractionPane();
        StartCoroutine(TalkingToRoutine());
        Debug.Log("[Begin announcement]");

        entities.Clear();

        foreach (Entity entity in this.GetComponent<Interactable>().dialogues[id].entities)
        {
            entities.Enqueue(entity);
        }

        Debug.Log("Dialogue size: " + entities.Count);
        DisplayNextSentence();

    }

    public void StartDialogue(Guard me) {
        eventNum = 0;
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
        if (eventNum != 0)
        {
            PlayEvent(eventNum);
        }
        if(entities.Count == 0) {
            EndDialogue();
            return;
        } else {

        }



        Entity entity = entities.Dequeue();
        eventNum = entity.eventNum;
        string sentence = entity.sentence;
        string name = entity.name;
        DialogueName.text = name;

        if(entity.image == null) {
            Face.gameObject.SetActive(false);
        } else {
            Face.gameObject.SetActive(true);
            Sprite face = entity.image;
            Face.sprite = entity.image;
        }

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

        //if (eventNum != 0)
        //{
        //    PlayEvent(eventNum);
        //}
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
        Level1.SetActive(true);
        WhereToGo.transform.position = new Vector2(-7.5f, -8f);
        PlayerRespawn = new Vector2(-3.5f, -7.5f);
        Announcement(0);
    }

    public void Level1_1() {
        WhereToGo.gameObject.transform.position = new Vector2(-0.86f, -1.65f);
        WhereToGo.Play();
    }

    public void Level1_2() {
        WhereToGo.gameObject.transform.position = new Vector2(-3.08f, 3.3f);
        WhereToGo.Play();
        FadeAnimator.SetBool("Fade", true);
        StartCoroutine(MakeBadGuyLeave1());
        PlayerRespawn = new Vector2(-1.0f, -1.0f);
    }
    public void Level1_3() {
        WhereToGo.gameObject.transform.position = new Vector2(-178f, 100f);
        WhereToGo.Play();
        // fade to black briefly
        FadeAnimator.SetBool("Fade", true);
        StartCoroutine(MakeBadGuyLeave1());
        PlayerRespawn = new Vector2(-1.0f, -1.0f);
    }


    public void Level2_2() {
        // pick up blue keycard
        GameStats.Blue = true;
    }

    public void ResetLevel1() {
        GameStats.CanMove = false;
        FadeAnimator.SetBool("Fade", true);
        StartCoroutine(Reset1());
    }

    public void SecurityOfficeDoor() {
        if(GameStats.Green) {
            // door is opened
            SecurityDoor.Play("Open");
        }
    }

    public void MakeHOSLeave1() {
        FadeAnimator.SetBool("Fade", true);
        StartCoroutine(MakeHOSLeave());
    }
    public void MakeGuardsLeave1() {
        FadeAnimator.SetBool("Fade", true);
        StartCoroutine(MakeGuardsLeave());
    }
    public void GoToElevator() {
        FadeAnimator.SetBool("Fade", true);
        //PlayerRespawn =
        StartCoroutine(GoToElev());
    }
    public void GoToLevel2() {
        // TODO make level 2
        FadeAnimator.SetBool("Fade", true);
        PlayerRespawn = new Vector2(-3f, -7f);
        StartCoroutine(GoTo2(true));
    }
    public void GoToLevel2From3_1() {
        FadeAnimator.SetBool("Fade", true);
        PlayerRespawn = new Vector2(-3.2f, 0.8f);
        StartCoroutine(GoTo2(false));
    }

    public void GoToLevel3_1From2() {
        FadeAnimator.SetBool("Fade", true);
        PlayerRespawn = new Vector2(18f, -7.67f);
        StartCoroutine(GoTo3_1(true));
    }

    public void GoToLevel3_1From3_2() {
        FadeAnimator.SetBool("Fade", true);
        PlayerRespawn = new Vector2(-32.3f, 2.2f);
        StartCoroutine(GoTo3_1(false));
    }
    public void GoToLevel4() {
        StartCoroutine(GoTo4());
    }
    public void Level3Drive() {
        WhereToGo.transform.position = new Vector2(-14.72f, 2.52f);
        WhereToGo.Play();
    }

    public void Level3Elevator() {
        WhereToGo.transform.position = new Vector2(-11.04f, 2.21f);
        WhereToGo.Play();
    }
    public void PlayEvent(int eventNum) {
        if (eventNum == 1)
        {
            Level1_1();
        }
        else if (eventNum == 2)
        {
            Level1_2();
        }
        else if (eventNum == 3)
        {
            Level1_3();
        }
        else if (eventNum == 4)
        {
            GoToLevel2();
        }
        else if (eventNum == 21)
        {
            GameStats.Green = true;
        }
        else if (eventNum == 22)
        {
            SecurityOfficeDoor();
        }
        else if (eventNum == 23)
        {
            GameStats.Blue = true;
        }
        else if (eventNum == 24)
        {
            GoToLevel3_1From2();
        }
        else if (eventNum == 20)
        {
            GameStats.KnowsBlue = true;
        }
        else if (eventNum == 30)
        {
            GoToLevel2From3_1();
        }
        else if (eventNum == 31)
        {
            MakeHOSLeave1();
        }
        else if (eventNum == 34)
        {
            MakeGuardsLeave1();
        }
        else if (eventNum == 38)
        {
            GameStats.ExtraEvidence1 = true;
        }
        else if (eventNum == 39)
        {
            Debug.Log("Here");
            StartCoroutine(JenkinsEntrance());
        }
        else if (eventNum == 40)
        {
            GoToLevel3_1From3_2();
        }
        else if (eventNum == 50)
        {
            Level3Drive();
        }
        else if(eventNum == 52) {
            Level3Elevator();   
        }
        else if (eventNum == 55) {
            // move to level 4
        }
        else if(eventNum == 90) {
            ResetLevel1();
        }
        else if(eventNum == 100) {
            // end game
            EndGame();
        }
    }

    public void EndGame() {
        GameStats.CanMove = false;
        GameStats.Blue = false;
        GameStats.Green = false;
        GameStats.IsStealthed = false;
        StartCoroutine(EndMe());

    }

    IEnumerator EndMe() {
        FadeAnimator.SetBool("Fade", true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("EndGame");
    }

    IEnumerator GoTo4() {
        FadeAnimator.SetBool("Fade", true);
        yield return new WaitForSeconds(1.0f);
        Level1.SetActive(false);
        Level2.SetActive(false);
        Level3_1.SetActive(false);
        Level3_2.SetActive(false);
        Level4.SetActive(true);
        FadeAnimator.SetBool("Fade", false);
        WhereToGo.transform.position = new Vector2(-10.23f, 4.21f);
        Announcement(3);
    }
    IEnumerator Reset1() {
        yield return new WaitForSeconds(1.0f);
        Player.transform.position = PlayerRespawn;
            //new Vector2(-3.5f, -7.5f);
        FadeAnimator.SetBool("Fade", false);
        GameStats.CanMove = true;
    }

    IEnumerator GoTo2(bool wanna) {
        yield return new WaitForSeconds(1.0f);
        Level1.SetActive(false);
        Level2.SetActive(true);
        Level3_1.SetActive(false);
        Level3_2.SetActive(true);
        Level4.SetActive(false);
        Player.transform.position = PlayerRespawn;
        FadeAnimator.SetBool("Fade", false);
        WhereToGo.Stop();
        if(wanna)
            Announcement(1);
    }

    IEnumerator GoTo3_1(bool wanna) {
        yield return new WaitForSeconds(1.0f);
        Level1.SetActive(false);
        Level2.SetActive(false);
        Level3_1.SetActive(true);
        Level3_2.SetActive(false);
        Level4.SetActive(false);
        Player.transform.position = PlayerRespawn;
        FadeAnimator.SetBool("Fade", false);
        WhereToGo.Stop();
        if(wanna)
            Announcement(2);
    }

    IEnumerator MakeBadGuyLeave1() {
        yield return new WaitForSeconds(1.0f);
        TheBadMan.SetActive(false);
        DoorGuard.SetActive(false);
        ExtraGuard.SetActive(true);
        FadeAnimator.SetBool("Fade", false);

    }

    IEnumerator MakeHOSLeave() {
        yield return new WaitForSeconds(1.0f);
        HeadOfSecurity.SetActive(false);
        FadeAnimator.SetBool("Fade", false);
    }

    IEnumerator MakeGuardsLeave() {
        yield return new WaitForSeconds(1.0f);
        Guards.SetActive(false);
        FadeAnimator.SetBool("Fade", false);
    }

    IEnumerator GoToElev() {
        yield return new WaitForSeconds(1.0f);
        Level1.SetActive(false);
        Level2.SetActive(false);
        Player.transform.position = PlayerRespawn;
        FadeAnimator.SetBool("Fade", false);
    }

    IEnumerator JenkinsEntrance() {
        FadeAnimator.SetBool("Fade", true);
        yield return new WaitForSeconds(1.0f);
        Jenkins.transform.position = new Vector2(-15f, 1.56f);
        Jenkins.GetComponent<Guard>().enabled = false;
        FadeAnimator.SetBool("Fade", false);
    }

    public void specialInstanceCheck() {
        if (currentInteractable.Type == 10)
        {
            if (GameStats.Green) {
                StartDialogue(currentInteractable, 1);
            }
            else {
                StartDialogue(currentInteractable, 0);
            }
        } else if(currentInteractable.Type == 11) {
            if(GameStats.Blue) {
                StartDialogue(currentInteractable, 1);
            }
            else {
                StartDialogue(currentInteractable, 0);
            }
        } else if(currentInteractable.Type == 12) {
            if(GameStats.HasDrive) {
                StartDialogue(currentInteractable, 1);
            } else {
                StartDialogue(currentInteractable, 0);
            }
        }
        else
        {
            StartDialogue(currentInteractable);
        }
    }

    void PlayMyClip()
    {
        if (!Theater.GetComponent<VideoPlayer>().isPlaying)
        {
            GameStats.CanMove = false;
            // Play clip
            Theater.GetComponent<VideoPlayer>().Play();
            // Wait for the clip to finish
            StartCoroutine(Wait(72f));

            //
            // Add your code here
            //
        }

    }

    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        Level1_0();
        GameStats.CanMove = true;
        Theater.SetActive(false);

    }
}
