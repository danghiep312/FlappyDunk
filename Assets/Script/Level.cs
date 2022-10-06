using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Asset/Level")]
[System.Serializable]
public class Level : ScriptableObject
{
    public int level;
    [TextArea]
    public string description;
    public GameObject levelPrefab;
}
