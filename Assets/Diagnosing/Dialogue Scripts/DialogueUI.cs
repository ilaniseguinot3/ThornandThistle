using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Portrait")]
    [SerializeField] private Image portraitImage;

    [Header("Continue Indicator")]
    [SerializeField] private GameObject pressSpaceIndicator;

    [Header("Click to Advance")]
    [SerializeField] private Button advanceButton;

    [SerializeField] private float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private string fullText;
    private bool skipRequested = false;

    private void Awake()
    {
        panel.SetActive(false);
        if (pressSpaceIndicator != null)
            pressSpaceIndicator.SetActive(false);
        if (advanceButton != null)
            advanceButton.onClick.AddListener(() =>
                DialogueManager.Instance.HandleAdvanceInput());
    }

    public void ShowLine(DialogueLine line, Action onTypingComplete = null)
    {
        panel.SetActive(true);
        skipRequested = false;

        if (pressSpaceIndicator != null)
            pressSpaceIndicator.SetActive(false);

        speakerText.text = line.speakerName;

        if (line.portrait != null)
        {
            portraitImage.sprite = line.portrait;
            portraitImage.enabled = true;
        }
        else
        {
            portraitImage.enabled = false;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line.text, onTypingComplete));
    }

    public void SkipTyping()
    {
        skipRequested = true;
    }

    private IEnumerator TypeText(string text, Action onComplete)
    {
        fullText = text;
        dialogueText.text = "";

        foreach (char c in text)
        {
            if (skipRequested)
            {
                dialogueText.text = fullText;
                break;
            }
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        skipRequested = false;
        if (pressSpaceIndicator != null)
            pressSpaceIndicator.SetActive(true);

        onComplete?.Invoke();
    }

    public void Hide()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        panel.SetActive(false);
        if (pressSpaceIndicator != null)
            pressSpaceIndicator.SetActive(false);
    }
}