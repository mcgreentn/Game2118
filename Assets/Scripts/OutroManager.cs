using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class OutroManager : MonoBehaviour {


    public Animator Top;
    public Animator Bottom;

    public Text TopText;
    public Text BottomText;
	// Use this for initialization
	void Start () {
        //StartCoroutine(OuttroPlay());
	}
	
    IEnumerator OuttroPlay() {
        TopText.text = "What have";
        BottomText.text = "I done...";
        yield return new WaitForSeconds(1.5f);
        Top.SetBool("FadeTop", false);
        yield return new WaitForSeconds(1.0f);
        Bottom.SetBool("FadeBottom", false);
        yield return new WaitForSeconds(3.0f);
        Top.SetBool("FadeTop", true);
        yield return new WaitForSeconds(0.5f);
        Bottom.SetBool("FadeBottom", true);
        yield return new WaitForSeconds(1.5f);


        TopText.text = "I always wanted to be";
        BottomText.text = "an agent of change.";
        yield return new WaitForSeconds(1.5f);
        Top.SetBool("FadeTop", false);
        yield return new WaitForSeconds(1.0f);
        Bottom.SetBool("FadeBottom", false);
        yield return new WaitForSeconds(3.0f);
        Top.SetBool("FadeTop", true);
        yield return new WaitForSeconds(0.5f);
        Bottom.SetBool("FadeBottom", true);
    }

    public void EndGame() {
        SceneManager.LoadScene("Menu");
    }
}
