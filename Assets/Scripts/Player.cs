using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    /// <summary>
    /// The move speed.
    /// </summary>
    public float moveSpeed;

    public SpriteRenderer player;
    private Animator anim;
    private Rigidbody2D myRigidBody;


    public Sprite[] playerImage;
    // Stealth Animator
    public Animator StealthAnimator;
    // Use this for initialization
    void Start()
    {
//        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(GameStats.CanMove) {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            myRigidBody.velocity = movement * moveSpeed;

            if(moveHorizontal > 0.0f) {
                anim.Play("Left");
                //player.sprite = playerImage[0];
            }
            if(moveHorizontal < 0.0f) {
                anim.Play("Right");
                player.sprite = playerImage[3];
            }
            if(moveVertical > 0.0f) {
                anim.Play("Up");
                player.sprite = playerImage[2];
            }
            if(moveVertical < 0.0f) {
                anim.Play("Down");
                player.sprite = playerImage[1];
            }


            //if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
            //{
            //    //transform.Translate(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f);
            //    player.sprite = playerImage[0];
            //    myRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*moveSpeed,myRigidBody.velocity.y);
            //}
            //if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
            //{
            //    //transform.Translate(0f,Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f);
            //    player.sprite = playerImage[1];
            //    myRigidBody.velocity = new Vector2(myRigidBody.velocity.x,Input.GetAxisRaw("Vertical")*moveSpeed);
            //}
            //if(Input.GetAxisRaw("Horizontal")<0.5f && Input.GetAxisRaw("Horizontal") > -0.5f)
            //{
            //    player.sprite = playerImage[2];
            //    myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
            //}
            //if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
            //{
            //    //transform.Translate(0f,Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f);
            //    player.sprite = playerImage[3];
            //    myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);
            //}

            if(Input.GetKeyDown(KeyCode.X)) {
                if(GameStats.IsStealthed) {

                    StealthOff();
                } else {

                    StealthOn();
                }
            }
        } else {
            myRigidBody.velocity = new Vector2(0f, 0f);

        }
    }


    void MakeTransparent(float alpha){
        player.color = new Color(1f, 1f, 1f, alpha/255f);
    }

    public void StealthOn()
    {
        Debug.Log("Player stealthed");
        GameStats.IsStealthed = true;
        MakeTransparent(180f);
        StealthAnimator.SetBool("Stealthed", true);
    }

    public void StealthOff()
    {
        Debug.Log("Player unstealthed");
        GameStats.IsStealthed = false;
        MakeTransparent(255f);
        StealthAnimator.SetBool("Stealthed", false);
    }
}
