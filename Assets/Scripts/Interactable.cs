using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public Dialogue[] dialogues;
    public Dialogue[] investigates;
    public Dialogue[] evesdroppings;
    public string name;
    public int counter;
    public Sprite image;
    private Manager M;
	private BoxCollider2D colliderbox;
    public int talking = 0;
    public int interacting = 0;

    public int Type = 0;

    public string interactPrompt;
	// Use this for initialization
	void Start () 
    {
		colliderbox = this.gameObject.GetComponent<BoxCollider2D> ();
        M = GameObject.FindWithTag("GameController").GetComponent<Manager>();
        if(interactPrompt.Equals("")) {
            interactPrompt = "Press space to interact. Press F to investigate.";
        }
	}

	 void Update()
	{

	}

	//void OnCollisionEnter2D(Collision2D other) 
 //   {
 //       M.ShowInteractionPane(name, interactPrompt, this);

	//}
	//void OnCollisionStay2D(Collision2D collision)
	//{
 //       if(M.interacting == 0 && M.talking == 0)
 //           M.ShowInteractionPane(name, interactPrompt, this);
	//}
	//void OnCollisionExit2D(Collision2D collision)
	//{
 //       M.HideInteractionPane();
 //       interacting = 0;
	//}

	void OnTriggerEnter2D(Collider2D collision)
	{
        M.ShowInteractionPane(name, interactPrompt, this);
        interacting = 1;
	}
    void OnTriggerStay2D(Collider2D collision)
    {
        if (M.interacting == 0 && M.talking == 0)
            M.ShowInteractionPane(name, interactPrompt, this);
    }
	void OnTriggerExit2D(Collider2D collision)
	{
        M.HideInteractionPane();
	}

    public void Collect() {
        if(Type == 1) {
            //flip the bit for this
        }
        // TODO add more collectables

        this.gameObject.SetActive(false);
    }

}
