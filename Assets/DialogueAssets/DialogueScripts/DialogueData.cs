using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine {
    public string speakerName;
    [TextArea(2, 5)] public string text;
    public Sprite portrait;
    public float autoAdvanceDelay = 2f; // seconds before next line
}

[CreateAssetMenu(fileName = "NewCustomerDialogue", menuName = "Dialogue/Customer Dialogue")]
public class DialogueData : ScriptableObject {
    public List<DialogueLine> lines;
}
