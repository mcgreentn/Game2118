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

    public int type;
    public int triggered;
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
        if ((!GameStats.IsStealthed && type!=1) || (type == 1 && triggered == 0))
        {
            M.StartDialogue(this);
            triggered = 1;
        }



    }
	private void OnTriggerStay2D(Collider2D collision)
	{
        if ((!GameStats.IsStealthed && type != 1) || (type == 1 && triggered == 0))
        {
            M.StartDialogue(this);
            triggered = 1;
        }
	}

	//private void OnCollisionEnter2D(Collision2D collision)
	//{
	//       if (!GameStats.IsStealthed)
	//       {
	//           M.StartDialogue(this);
	//       }
	//}


}
