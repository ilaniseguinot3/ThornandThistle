using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance { get; private set; }

    public DialogueUI dialogueUI;
    private DialogueData currentDialogue;
    private int currentLineIndex = 0;
    private bool isPlaying = false;
    private bool open = true;

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
        if (currentDialogue == null || currentLineIndex >= currentDialogue.lines.Count) 
        {
            // wait until user closes dialogue
            StartCoroutine(wait());
            return;
        }

        var line = currentDialogue.lines[currentLineIndex];
        dialogueUI.ShowLine(line);
        StartCoroutine(AutoNext(line.autoAdvanceDelay));
    }

    // wait until user closes dialogue
    IEnumerator wait()
    {
        while (open)
            {
                var key = Keyboard.current;
                // if nothing is being pressed, return
                if (key == null)
                    yield break;
                // otherwise if space is being pressed close dialogue
                else if (key.spaceKey.isPressed)
                    open = false;

                // wait for the next frame
                yield return null;
            }
            open = true;
            EndDialogue();
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
