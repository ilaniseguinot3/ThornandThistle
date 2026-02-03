using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Item Data")]
    public string itemName = "Ingredient";
    public string itemDescription = "A collectible ingredient";
    public Sprite itemIcon;
    public IngredientType ingredientType = IngredientType.Fire;

    [Header("Collectible Settings")]
    public bool autoCollect = false;
    public float autoCollectRange = 2f;
    public GameObject collectEffect;
    public AudioClip collectSound;

    [Header("Visual Settings")]
    public bool rotateItem = true;
    public float rotationSpeed = 50f;
    public bool bobUpAndDown = true;
    public float bobHeight = 0.3f;
    public float bobSpeed = 2f;

    private Vector3 startPosition;
    private Transform playerTransform;
    private AudioSource audioSource;

    private void Start()
    {
        startPosition = transform.position;
        
        // Find player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Setup audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && collectSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        // Rotate the item
        if (rotateItem)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        // Bob up and down
        if (bobUpAndDown)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        // Auto collect if player is nearby
        if (autoCollect && playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance <= autoCollectRange)
            {
                Collect();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    // This can be called when player clicks on the object
    private void OnMouseDown()
    {
        Collect();
    }

    public void Collect()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager not found!");
            return;
        }

        // Create inventory item
        InventoryItem newItem = new InventoryItem(
            itemName,
            itemDescription,
            itemIcon,
            ingredientType
        );

        // Try to add to inventory
        bool added = InventoryManager.Instance.AddItem(newItem);

        if (added)
        {
            // Play collection effects
            if (collectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectSound);
            }

            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            // Destroy the collectible
            Destroy(gameObject, collectSound != null ? collectSound.length : 0f);
        }
    }

    // Optional: Draw gizmo to show auto-collect range
    private void OnDrawGizmosSelected()
    {
        if (autoCollect)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, autoCollectRange);
        }
    }
}