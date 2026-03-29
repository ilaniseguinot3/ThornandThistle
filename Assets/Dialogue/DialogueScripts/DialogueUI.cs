using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueUI : MonoBehaviour {
    [SerializeField] private GameObject panel;
    [SerializeField] private TMPro.TextMeshProUGUI speakerText;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
    [SerializeField] private Image portraitImage;

    [SerializeField] private float typingSpeed = 0.03f;

    void Awake() {
        panel.SetActive(false);
    }

    public void ShowLine(DialogueLine line) {
        panel.SetActive(true);
        speakerText.text = line.speakerName;
        portraitImage.sprite = line.portrait;
        StopAllCoroutines();
        StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string text) {
        dialogueText.text = "";
        foreach (char c in text) {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void Hide() {
        panel.SetActive(false);
    }
}
