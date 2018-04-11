using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour
{


    public Dialogue dialogue;
    public Sprite image;
    private Manager M;
    private BoxCollider2D colliderbox;
    public int talking = 0;
    public int interacting = 0;


	// Use this for initialization
	void Start()
	{
        colliderbox = this.gameObject.GetComponent<BoxCollider2D>();
        M = GameObject.FindWithTag("GameController").GetComponent<Manager>();
	}

	// Update is called once per frame
	void Update()
	{
			
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameStats.IsStealthed)
        {
            M.StartDialogue(this);
        }
    }


}
