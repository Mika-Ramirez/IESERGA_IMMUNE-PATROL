using UnityEngine;
using System.Collections;
using TMPro;

public class Note : MonoBehaviour
{
    [SerializeField] private bool canBePressed = false;
    [SerializeField] private KeyCode keyToPress;
    [SerializeField] private Transform activatorTransform; 
    [SerializeField] private AudioClip drumClip;
    [SerializeField] private float animationDuration = 0.05f; // Duration for the scale animation
    [SerializeField] private float maxScale = 2.0f; // How big the note gets before disappearing

     // Reference to feedback text component
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float feedbackDisplayTime = 1.0f; // How long feedback stays on screen

    private AudioSource audioSource;
    private bool isHit = false; // Prevent multiple presses
    private bool isBeingDestroyed = false; // Flag to track destruction process

    private PlayerStats playerStats;
    void Start()
    {
        // Safely register with GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterNote(this);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Ensure AudioSource exists
        }

        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress) && canBePressed && !isHit)
        {
            isHit = true; // Mark as hit to prevent multiple presses
            PlayNote();
        }
    }

    private void PlayNote()
    {
        if (SoundManager.Instance != null && drumClip != null && audioSource != null)
        {
            SoundManager.Instance.PlayClip(drumClip, audioSource);
        }

        if (activatorTransform != null)
        {
            float distance = Mathf.Abs(transform.position.x - activatorTransform.position.x);
            string feedbackMessage = "";

            if (distance < 0.3f)
            {
                feedbackMessage = "Perfect!";
                Debug.Log(feedbackMessage);
            }
            else if (distance < 0.5f)
            {
                feedbackMessage = "Great!";
                Debug.Log(feedbackMessage);
            }
            else if (distance < 0.81f)
            {
                feedbackMessage = "Good!";
                Debug.Log(feedbackMessage);
            }
            
            // Display feedback in game
            playerStats.AddNoteScore(feedbackMessage);
            GameManager.Instance.DisplayFeedback(feedbackMessage);
        }

        AnimateAndDestroy();
    }
    
    private void DisplayFeedback(string message)
    {
        // Make sure we have a reference to the feedback text
        if (feedbackText != null)
        {
            // Set text and make visible
            feedbackText.text = message;
            feedbackText.gameObject.SetActive(true);
            
            // Optional: Animate the text
            feedbackText.transform.localScale = Vector3.one * 1.5f;
            LeanTween.scale(feedbackText.gameObject, Vector3.one, 0.2f).setEase(LeanTweenType.easeOutBack);
            
            // Start coroutine to hide the text after a delay
            StopCoroutine("HideFeedbackAfterDelay");
            StartCoroutine(HideFeedbackAfterDelay());
        }
        else
        {
            Debug.LogWarning("Feedback text reference not set in Note script!");
        }
    }
    
    private IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(feedbackDisplayTime);
        if (feedbackText != null)
        {
            feedbackText.gameObject.SetActive(false);
        }
    }

    public void AnimateAndDestroy()
    {
        // Cancel any existing tweens on this object
        LeanTween.cancel(gameObject);
        
        // Store original scale to reset if needed
        Vector3 originalScale = transform.localScale;
        
        // Animate scale up with longer duration for visibility
        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.05f) // Try a longer duration like 0.3f
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() => {
                StartCoroutine(DestroyAfterSound());
            });
            
    }

    private IEnumerator DestroyAfterSound()
    {
        // Safely disable visual components
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.enabled = false; // Hide note visually
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Disable further collision
        }

        // Wait for sound to finish if clip exists
        if (drumClip != null)
        {
            yield return new WaitForSeconds(drumClip.length);
        }
        else
        {
            yield return null;
        }

        DestroyNote();
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Activator"))
        {
            canBePressed = true;
            activatorTransform = other.transform; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Activator"))
        {
            canBePressed = false;
        }
    }

    private void OnDestroy()
    {
        // Prevent double cleanup and check if GameManager still exists
        if (!isBeingDestroyed && GameManager.Instance != null)
        {
            GameManager.Instance.RemoveNote(this);
        }
    }

    private void DestroyNote()
    {
        // Set flag to prevent double cleanup in OnDestroy
        isBeingDestroyed = true;
        
        // Safely remove from GameManager if it exists
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RemoveNote(this);
        }
        
        Destroy(gameObject);
    }
}