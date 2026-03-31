using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI")]
    public DialogueUI dialogueUI;

    private DialogueData currentDialogue;
    private int currentLineIndex = 0;
    private bool isPlaying = false;
    private bool waitingForInput = false;
    private bool isTyping = false;
    private Action onCompleteCallback;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        if (!isPlaying) return;
        if (Keyboard.current == null) return;
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            HandleAdvanceInput();
    }

    public void HandleAdvanceInput()
    {
        if (!isPlaying) return;
        if (isTyping)
            dialogueUI.SkipTyping();
        else if (waitingForInput)
        {
            waitingForInput = false;
            AdvanceLine();
        }
    }

    public void StartDialogue(DialogueData dialogue, Action onComplete = null)
    {
        if (isPlaying)
        {
            Debug.LogWarning("⚠️ Dialogue already playing");
            return;
        }
        if (dialogue == null)
        {
            Debug.LogError("❌ DialogueData is null!");
            return;
        }

        currentDialogue = dialogue;
        currentLineIndex = 0;
        isPlaying = true;
        onCompleteCallback = onComplete;
        ShowCurrentLine();
    }

    public bool IsPlaying() => isPlaying;

    private void ShowCurrentLine()
    {
        if (currentDialogue == null || currentLineIndex >= currentDialogue.lines.Count)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = currentDialogue.lines[currentLineIndex];
        isTyping = true;
        waitingForInput = false;

        dialogueUI.ShowLine(line, onTypingComplete: () =>
        {
            isTyping = false;
            waitingForInput = true;
        });
    }

    private void AdvanceLine()
    {
        currentLineIndex++;
        ShowCurrentLine();
    }

    private void EndDialogue()
    {
        isPlaying = false;
        isTyping = false;
        waitingForInput = false;
        currentDialogue = null;
        dialogueUI.Hide();
        onCompleteCallback?.Invoke();
        onCompleteCallback = null;
        Debug.Log("✅ Dialogue ended");
    }

    public void CancelDialogue()
    {
        StopAllCoroutines();
        isPlaying = false;
        isTyping = false;
        waitingForInput = false;
        currentDialogue = null;
        onCompleteCallback = null;
        dialogueUI.Hide();
    }
}