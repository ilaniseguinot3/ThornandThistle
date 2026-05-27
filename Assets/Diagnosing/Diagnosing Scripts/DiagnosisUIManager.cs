using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiagnosisUIManager : MonoBehaviour
{
    public static DiagnosisUIManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject diagnosisPanel;

    // customer image parts
    public Image skinImage;
    public Image shirtImage;
    public Image eyesImage;
    public Image noseImage;
    public Image mouthImage;
    public Image hairImage;
    public Image accessoryImage;

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

        //parts of customer
        if (/*customerPortraitImage != null &&*/ CustomerManager.Instance.CurrentCustomer != null)
        {
            skinImage.sprite = CustomerManager.Instance.CurrentCustomer.skin;
            shirtImage.sprite = CustomerManager.Instance.CurrentCustomer.shirt;
            eyesImage.sprite = CustomerManager.Instance.CurrentCustomer.eyes;
            noseImage.sprite = CustomerManager.Instance.CurrentCustomer.nose;
            mouthImage.sprite = CustomerManager.Instance.CurrentCustomer.mouth;
            hairImage.sprite = CustomerManager.Instance.CurrentCustomer.hair;
            accessoryImage.sprite = CustomerManager.Instance.CurrentCustomer.accessory;
        }
        /*
        if (customerPortraitImage != null && CustomerManager.Instance.CurrentCustomer != null)
        {
            customerPortraitImage.sprite = CustomerManager.Instance.CurrentCustomer.portrait;
            customerPortraitImage.enabled = true;
        }
        */
    }

    public void HideIllness()
    {
        diagnosisPanel.SetActive(false);
    }
}