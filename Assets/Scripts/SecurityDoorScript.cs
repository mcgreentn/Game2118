using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityDoorScript : MonoBehaviour {

    public AudioSource clip;
    public void DeleteMe() {
        this.gameObject.SetActive(false);
    }

    public void PlaySound() {
        clip.Play();
    }
}
