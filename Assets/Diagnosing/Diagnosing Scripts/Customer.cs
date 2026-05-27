using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "Diagnosis/Customer")]
public class Customer : ScriptableObject
{
    //old 
    //public Sprite portrait;

    //randomize:
    public string customerName;
    public Sprite skin;
    public Sprite shirt;
    public Sprite eyes;
    public Sprite nose;
    public Sprite mouth;
    public Sprite hair;
    public Sprite accessory;
    
    // must attach dialogues to illness
    public Illness illness;

    [Header("Dialogues")]
    public DialogueData arrivalDialogue;       // first door click
    public DialogueData returnDialogue;        // second door click — "what have you brought me?"
    public DialogueData correctPotionDialogue; // right potion given
    public DialogueData wrongPotionDialogue;   // wrong potion given
}