using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource soundsource;
    [SerializeField] private AudioClip[] countdownClips;
    [SerializeField] private Sprite[] countdownSprites; // Array of countdown sprites (3,2,1,Go)
    [SerializeField] private Image countdownDisplay; // UI Image to show countdown sprites
    [SerializeField] private float countdownDelay = 1.0f; // Delay between countdown numbers
    [SerializeField] private GameObject countdownCanvas; // Parent canvas for countdown UI
    
    // Reference to feedback text component
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float feedbackDisplayTime = 1.0f; // How long feedback stays on screen
    private bool isPlayerDead = false;
    private bool playing = false;
    public static event Action startPlaying; // Event for game start
    public static event Action stopPlaying;
    private List<Note> activeNotes = new List<Note>();
    private PlayerStats playerStats;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {   
        // Hide countdown UI initially
        if (countdownCanvas != null)
            countdownCanvas.SetActive(false);
        
        if(feedbackText != null && feedbackText.gameObject.activeSelf)
        {
            feedbackText.gameObject.SetActive(false);
        }

        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    void Update()
    {
        if (!playing && Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Starting countdown");
            StartCoroutine(StartCountdown());
        }
        if (playing && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("stopped");
            StartCoroutine(StopPlaying());
        }
    }

    public IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(1.5f);
        // Show countdown UI
        if (countdownCanvas != null)
            countdownCanvas.SetActive(true);
            
        // Play countdown audio
        // if (countdownAudio != null)
        //     countdownAudio.Play();
            
        // Make sure the sprite count matches the audio clip count
        int countToUse = Mathf.Min(countdownSprites.Length, countdownClips.Length);
        
        // Play each countdown clip and show corresponding sprite
        for (int i = 0; i < countToUse; i++)
        {
            if (countdownDisplay != null && countdownSprites[i] != null)
            {
                // Get the native size of the sprite before changing it
                Vector2 spriteSize = new Vector2(
                    countdownSprites[i].rect.width,
                    countdownSprites[i].rect.height
                );
                
                // Set the new sprite
                countdownDisplay.sprite = countdownSprites[i];
                
                // Set preserve aspect ratio to false for exact size control
                countdownDisplay.preserveAspect = false;
                
                // Apply the sprite's native size to the display's RectTransform
                countdownDisplay.rectTransform.sizeDelta = spriteSize;
                
                // Animate the countdown number
                countdownDisplay.transform.localScale = Vector3.one * 1.5f;
                LeanTween.scale(countdownDisplay.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);
            }
            
            // Play the current audio clip
            if (countdownClips[i] != null)
            {
                SoundManager.Instance.PlayClip(countdownClips[i], soundsource);
                
                // Wait until this clip finishes playing
                float clipDuration = countdownClips[i].length;
                yield return new WaitForSeconds(clipDuration);
            }
            else
            {
                // Fallback if clip is missing
                yield return new WaitForSeconds(1.0f);
            }
        }
        
        // Hide countdown UI
        if (countdownCanvas != null)
            countdownCanvas.SetActive(false);
            
        // Start the actual game
        StartCoroutine(StartPlaying());
    }

    private IEnumerator StartPlaying()
    {
        playing = true;
        music.Play();

        // Wait until the music starts playing
        yield return new WaitUntil(() => music.isPlaying);

        startPlaying?.Invoke(); 

        float maxScore = playerStats.MaxScore;
        playerStats.pointsPerNote = maxScore / activeNotes.Count;

        Debug.Log(playerStats.pointsPerNote);
        Debug.Log(activeNotes.Count);

    }

    public IEnumerator StopPlaying()
    {
        playing = false;
        music.Pause();

        yield return new WaitUntil(() => !music.isPlaying);

        stopPlaying?.Invoke(); 
    }

    public void RegisterNote(Note note)
    {
        if (note != null)
            activeNotes.Add(note); 
    }

    public void RemoveNote(Note note)
    {
        if (note != null && activeNotes.Contains(note))
            activeNotes.Remove(note);
            
        StartCoroutine(CheckAfterWaitTime());
    }

    public List<Note> GetActiveNotes()
    {
        return activeNotes; 
    }

    private void CheckForAllFinishedNotes()
    {
        if(activeNotes.Count == 0)
        {
            WinGame();
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(3f);
        CheckForAllFinishedNotes();
    }

    private void WinGame()
    {
        Debug.Log("WinGame");
        // Add your win game logic here
        UIManager.Instance.UpdateGameOverScreen();
        StartCoroutine(StopPlaying());
    }

    public void LoseGame()
    {     
        isPlayerDead = true;
        
        UIManager.Instance.UpdateGameOverScreen();
        
        StartCoroutine(StopPlaying());
    }
    public void RestartGame()
    {
        // Stop any running coroutines
        StopAllCoroutines();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void NextLevel()
    {
        // Stop any running coroutines
        StopAllCoroutines();

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene in build settings!");
            // Optionally go back to menu or restart
            // SceneManager.LoadScene(0); 
        }
    }

    public void BackToMenu()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(0); // 0 is the first scene in the Build Settings
    }

    public void DisplayFeedback(string message)
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
    public bool IsPlayerDead => isPlayerDead;
}