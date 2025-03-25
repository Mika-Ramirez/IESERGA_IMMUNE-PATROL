using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour
{
    [SerializeField] private bool canBePressed = false;
    [SerializeField] private KeyCode keyToPress;
    [SerializeField] private Transform activatorTransform; 
    [SerializeField] private AudioClip drumClip;

    private AudioSource audioSource;
    private bool isHit = false; // Prevent multiple presses

    void Start()
    {
        GameManager.Instance.RegisterNote(this); 

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Ensure AudioSource exists
        }
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
        SoundManager.Instance.PlayClip(drumClip, audioSource); // Play sound

        float distance = Mathf.Abs(transform.position.x - activatorTransform.position.x);

        if (distance < 0.2f)
        {
            Debug.Log("Perfect!");
        }
        else if (distance < 0.4f)
        {
            Debug.Log("Great!");
        }
        else if (distance < 0.6f)
        {
            Debug.Log("Good!");
        }

        StartCoroutine(DestroyAfterSound());
    }

    private IEnumerator DestroyAfterSound()
    {
        GetComponent<SpriteRenderer>().enabled = false; // Hide note visually
        GetComponent<Collider2D>().enabled = false; // Disable further collision

        yield return new WaitForSeconds(drumClip.length); // Wait for sound to finish

        DestroyNote();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            canBePressed = true;
            activatorTransform = other.transform; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            canBePressed = false;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveNote(this);
    }

    private void DestroyNote()
    {
        GameManager.Instance.RemoveNote(this);
        Destroy(gameObject);
    }
}
