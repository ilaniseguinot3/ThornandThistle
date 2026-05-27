using UnityEngine;
using System.Collections.Generic;

// dialogue set object - holds matching dialogues
[System.Serializable]
public class IllnessDialogueSet
{
    public DialogueData arrivalDialogue;
    public DialogueData returnDialogue;
    public DialogueData correctPotionDialogue;
    public DialogueData wrongPotionDialogue;
}

// illness class
[CreateAssetMenu(fileName = "New Illness", menuName = "Diagnosis/Illness")]
public class Illness : ScriptableObject
{
    public string illnessName;
    [TextArea(2, 4)] public string description;
    public Sprite illnessSprite;
    public Potion cure;

    // list of possible dialogues
    public List<IllnessDialogueSet> dialogueSets;
}