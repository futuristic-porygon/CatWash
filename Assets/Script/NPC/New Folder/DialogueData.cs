using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "NPC/Dialogue")]
public class DialogueData : ScriptableObject
{
    [TextArea(2, 5)]
    public List<string> lines;
}
