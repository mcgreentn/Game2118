using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public Dialogue[] dialogues;
    public string name;
    public int counter;
    public Sprite image;
    private Manager M;
	private BoxCollider2D colliderbox;
    public int talking = 0;
    public int interacting = 0;

    public int Type = 0;
	// Use this for initialization
	void Start () 
    {
		colliderbox = this.gameObject.GetComponent<BoxCollider2D> ();
        M = GameObject.FindWithTag("GameController").GetComponent<Manager>();
	}

	 void Update()
	{

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

    public void Collect() {
        if(Type == 1) {
            //flip the bit for this
        }
        // TODO add more collectables

        this.gameObject.SetActive(false);
    }

}
