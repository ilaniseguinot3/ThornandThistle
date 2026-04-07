using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiagnosisUIManager : MonoBehaviour
{
    public static DiagnosisUIManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject diagnosisPanel;
    public Image illnessImage;
    public TextMeshProUGUI illnessNameText;
    public TextMeshProUGUI illnessDescriptionText;
    public Image customerPortraitImage; // optional — shown during brewing phase

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

        if (customerPortraitImage != null && CustomerManager.Instance.CurrentCustomer != null)
        {
            customerPortraitImage.sprite = CustomerManager.Instance.CurrentCustomer.portrait;
            customerPortraitImage.enabled = true;
        }
    }

    public void HideIllness()
    {
        diagnosisPanel.SetActive(false);
    }
}