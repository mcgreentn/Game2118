using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    private Animator anim;
    private Rigidbody2D myRigidBody;
    // Use this for initialization
    void Start()
    {
//        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   if(GameStats.CanMove) {
            if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                //transform.Translate(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f);
                myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*moveSpeed,myRigidBody.velocity.y);
            }
            if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
            {
                //transform.Translate(0f,Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f);
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x,Input.GetAxisRaw("Vertical")*moveSpeed);
            }
            if(Input.GetAxisRaw("Horizontal")<0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
            {
                myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
            }
            if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
            {
                //transform.Translate(0f,Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f);
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
            }
            //anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
            //anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        } else {
            myRigidBody.velocity = new Vector2(0f, 0f);
        }
    }
}
