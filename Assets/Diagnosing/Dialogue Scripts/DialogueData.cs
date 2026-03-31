using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines = new();
    public bool autoCloseOnLastLine = false;
    public bool freezeOnLastLine = false;
}