using UnityEngine;

[CreateAssetMenu(fileName = "New Illness", menuName = "Diagnosis/Illness")]
public class Illness : ScriptableObject
{
    public string illnessName;
    [TextArea(2, 4)] public string description;
    public Sprite illnessSprite;
    public Potion cure;
}