using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance { get; private set; }

    public DialogueUI dialogueUI;
    private DialogueData currentDialogue;
    private int currentLineIndex = 0;
    private bool isPlaying = false;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartDialogue(DialogueData dialogue) {
        if (isPlaying) return;
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isPlaying = true;
        ShowLine();
    }

    void ShowLine() {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.lines.Count) {
            EndDialogue();
            return;
        }

        var line = currentDialogue.lines[currentLineIndex];
        dialogueUI.ShowLine(line);
        StartCoroutine(AutoNext(line.autoAdvanceDelay));
    }

    IEnumerator AutoNext(float delay) {
        yield return new WaitForSeconds(delay);
        currentLineIndex++;
        ShowLine();
    }

    void EndDialogue() {
        dialogueUI.Hide();
        isPlaying = false;
        currentDialogue = null;
    }
}
