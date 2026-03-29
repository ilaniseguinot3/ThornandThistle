using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "Diagnosis/Customer")]
public class Customer : ScriptableObject
{
    public string customerName;
    public Sprite portrait;                    // 👈 add this
    public Illness illness;
    public DialogueData arrivalDialogue;
    public DialogueData correctPotionDialogue;
    public DialogueData wrongPotionDialogue;
}