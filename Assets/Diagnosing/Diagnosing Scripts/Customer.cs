using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "Diagnosis/Customer")]
public class Customer : ScriptableObject
{
    public string customerName;
    public Sprite portrait;
    public Illness illness;

    [Header("Dialogues")]
    public DialogueData arrivalDialogue;       // first door click
    public DialogueData returnDialogue;        // second door click — "what have you brought me?"
    public DialogueData correctPotionDialogue; // right potion given
    public DialogueData wrongPotionDialogue;   // wrong potion given
}