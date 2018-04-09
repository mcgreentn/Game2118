using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueNode
{

    /// <summary>
    /// The npc's line.
    /// </summary>
    private string npcline;
    /// <summary>
    /// The player's line.
    /// </summary>
    private string playerline;

    /// <summary>
    /// The children for this node.
    /// </summary>
    private List<DialogueNode> children;

    private DialogueNode soleChild;

    private bool isVisible;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:DialogueNode"/> class.
    /// </summary>
    public DialogueNode() {
        children = new List<DialogueNode>();
    }

    /// <summary>
    /// Gets the children.
    /// </summary>
    /// <returns>The children.</returns>
    public List<DialogueNode> GetChildren() {
        return children;
    }

    /// <summary>
    /// Adds the child to the children list.
    /// </summary>
    /// <param name="child">Child.</param>
    public void addChild(DialogueNode child) {
        children.Add(child);
    }

    public bool IsVisible() {
        return isVisible;
    }

    public void ChangeVisibility(bool change) {
        isVisible = change;
    }

    public void AddNpcLine(string text) {
        npcline = text;
    }

    public string GetNpcLine(string text) {
        return npcline;
    }

    public void AddPlayerLine(string text) {
        playerline = text;
    }

    public string GetPlayerLine() {
        return playerline;
    }

    public void SetChild(DialogueNode child) {
        soleChild = child;
    }

    public DialogueNode GetChild() {
        return soleChild;
    }
}
