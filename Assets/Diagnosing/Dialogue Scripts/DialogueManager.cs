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
    private bool isFrozen = false;
    public GameObject crosshairs;
    private Action onCompleteCallback;
    private Action onFrozenCallback;
    public playerMovementScript playerMovementMouse;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        if (!isPlaying) return;
        if (isFrozen) return;
        if (Keyboard.current == null) return;
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            HandleAdvanceInput();
    }

    public void HandleAdvanceInput()
    {
        if (!isPlaying) return;
        if (isFrozen) return;
        if (isTyping)
            dialogueUI.SkipTyping();
        else if (waitingForInput)
        {
            waitingForInput = false;
            AdvanceLine();
        }
    }

    public void StartDialogue(DialogueData dialogue, Action onComplete = null, Action onFrozen = null)
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
        isFrozen = false;
        onCompleteCallback = onComplete;
        onFrozenCallback = onFrozen;

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;


        ShowCurrentLine();
    }

    /// <summary>
    /// Swaps to a new dialogue without closing the panel at all.
    /// </summary>
    public void SwapDialogue(DialogueData dialogue, Action onComplete = null)
    {
        StopAllCoroutines();
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isPlaying = true;
        isFrozen = false;
        isTyping = false;
        waitingForInput = false;
        onCompleteCallback = onComplete;
        onFrozenCallback = null;

        Debug.Log($"🔄 Swapping to dialogue: {dialogue.name}");
        ShowCurrentLine();
    }

    public bool IsPlaying() => isPlaying;
    public bool IsFrozen() => isFrozen;

    public void UnfreezeAndClear()
    {
        isFrozen = false;
        isPlaying = false;
        isTyping = false;
        waitingForInput = false;
        currentDialogue = null;
        onFrozenCallback = null;
        dialogueUI.Hide();
        Debug.Log("🧊 Dialogue unfrozen and cleared");
    }

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

            bool isLastLine = currentLineIndex >= currentDialogue.lines.Count - 1;

            if (isLastLine && currentDialogue.freezeOnLastLine)
            {
                isFrozen = true;
                Debug.Log("🧊 Dialogue frozen — waiting for potion selection");
                onFrozenCallback?.Invoke();
                onFrozenCallback = null;
            }
            else if (isLastLine && currentDialogue.autoCloseOnLastLine)
            {
                EndDialogue();
            }
            else
            {
                waitingForInput = true;
            }
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
        isFrozen = false;
        currentDialogue = null;
        dialogueUI.Hide();
        onCompleteCallback?.Invoke();
        onCompleteCallback = null;
        onFrozenCallback = null;
        playerMovementMouse.activeMouse = true;
        crosshairs.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("✅ Dialogue ended");
    }

    public void CancelDialogue()
    {
        StopAllCoroutines();
        isPlaying = false;
        isTyping = false;
        waitingForInput = false;
        isFrozen = false;
        currentDialogue = null;
        onCompleteCallback = null;
        onFrozenCallback = null;
        playerMovementMouse.activeMouse = true;
        crosshairs.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        dialogueUI.Hide();
    }
}