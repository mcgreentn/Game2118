using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public Dialogue dialogue;
    public Sprite image;
    private Manager M;
	private BoxCollider2D colliderbox;
    public int talking = 0;
    public int interacting = 0;
	// Use this for initialization
	void Start () 
    {
		colliderbox = this.gameObject.GetComponent<BoxCollider2D> ();
        M = GameObject.FindWithTag("GameController").GetComponent<Manager>();
	}

	 void Update()
	{

        //if (Input.GetKeyDown(KeyCode.Space) && interacting == 1)
        //{
        //    M.HideInteractionPane();
        //    TriggerDialogue();
        //} 
	}

	void OnCollisionEnter2D(Collision2D other) 
    {
        M.ShowInteractionPane(name, "Press space to interact...", this);	
	}

	void OnCollisionExit2D(Collision2D collision)
	{
        M.HideInteractionPane();
        interacting = 0;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        M.ShowInteractionPane(name, "Press space to interact...", this);
        interacting = 1;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        M.HideInteractionPane();
	}

    public void TriggerDialogue() {
        M.ShowDialoguePane(dialogue.name, image);
        Debug.Log("Starting dialogue up: " + this.name);
        M.StartDialogue(this);
    }
}
