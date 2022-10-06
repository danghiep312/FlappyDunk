using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "Asset/Skin")]
public class Skin : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public Sprite secondSprite; //for the second part of the skin
    public string tag;
    [TextArea]
    public string description;

    public Sprite starOfHoop;
    
    public bool progress;

    public Color colorOfFlame;
}
