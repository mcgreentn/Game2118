using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DialogueManager : MonoBehaviour
{
    public Manager M;

    public Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();


    }
}
