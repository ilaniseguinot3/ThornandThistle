using UnityEngine;
using UnityEngine.UI;

public class DiagnosisUIManager : MonoBehaviour
{
    public static DiagnosisUIManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject diagnosisPanel;  // the whole panel to show/hide
    public Image illnessImage;         // shows the illness sprite
    public TMPro.TextMeshProUGUI illnessNameText;
    public TMPro.TextMeshProUGUI illnessDescriptionText;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        diagnosisPanel.SetActive(false);
    }

    public void ShowIllness(Illness illness)
    {
        diagnosisPanel.SetActive(true);
        illnessImage.sprite = illness.illnessSprite;
        illnessNameText.text = illness.illnessName;
        illnessDescriptionText.text = illness.description;
    }

    public void HideIllness()
    {
        diagnosisPanel.SetActive(false);
    }
}