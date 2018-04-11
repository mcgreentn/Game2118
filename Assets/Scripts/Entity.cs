using UnityEngine;
using System.Collections;

[System.Serializable]
public class Entity
{
    public string name;

    [TextArea(3, 10)]
    public string sentence;
    public Sprite image;
}
