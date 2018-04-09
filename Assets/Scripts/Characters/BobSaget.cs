using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BobSaget : MonoBehaviour
{
    public DialogueNode root;

    public Manager M;


    public Queue<string> sentences;

	private void Start()
	{
        sentences = new Queue<string>();


        M = GameObject.FindWithTag("GameController").GetComponent<Manager>();
        RefreshTree();
	}
	public void RefreshTree() 
    {
        root = new DialogueNode();
        root.AddNpcLine("How can I help you?");
        root.AddPlayerLine("Wait I have more questions");

        // first layer
        DialogueNode oneOne = new DialogueNode();
        root.addChild(oneOne);
        oneOne.AddNpcLine("I am Bob Saget.");
        oneOne.AddPlayerLine("Who are you?");

        DialogueNode twoOne = new DialogueNode();
        root.addChild(twoOne);
        twoOne.AddNpcLine("I'm an actor! Haven't you heard of me?");
        twoOne.AddPlayerLine("What do you do?");
        twoOne.addChild(root);
    }

    public void BuildTree() {
        root = new DialogueNode();
        root.AddNpcLine("I'm Bob Saget.");

        DialogueNode one = new DialogueNode();
        one.AddNpcLine("I am an actor! Haven't you heard of me?");

        root.addChild(one);
    }

    public DialogueNode NextNode(DialogueNode node) {
        if(node.GetChild() != null) {
            return node.GetChild();
        }
        else {
            return null;
        }
    }


}

