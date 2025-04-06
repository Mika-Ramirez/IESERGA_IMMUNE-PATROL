using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton pattern
    public static UIManager Instance { get; private set; }

    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private float timeToDrain = 0.5f;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject gameOverScreen;
    private float currentDisplayScore = 0;
    private float targetScore = 0;
    private float scoreAnimationSpeed; // Points per second to increment \
    private bool isAnimatingScore = false;
    private PlayerStats playerStats;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Find player stats if not assigned
        if (playerStats == null)
        {
            playerStats = FindFirstObjectByType<PlayerStats>();
        }
        // Subscribe to PlayerStats events
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += UpdateHealthUI;
            playerStats.OnScoreChanged += UpdateScoreUI;
        }
        else
        {
            Debug.LogError("UIManager could not find PlayerStats component!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events when destroyed
        if (playerStats != null)
        {
            playerStats.OnHealthChanged -= UpdateHealthUI;
            playerStats.OnScoreChanged -= UpdateScoreUI;
        }
    }

    // UI update methods
    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            // Calculate target value
            float targetFill = currentHealth / maxHealth;
            
            // Use LeanTween to animate the fill amount
            LeanTween.value(healthBar.gameObject, healthBar.fillAmount, targetFill, timeToDrain)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnUpdate((float val) => {
                    healthBar.fillAmount = val;
                });
        }

        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}/{maxHealth}";
        }
    }

    private void UpdateScoreUI(float newScore)
    {
        // Update the target score
        targetScore = newScore;
        scoreAnimationSpeed = playerStats.pointsPerNote * 2;
        // Start animation coroutine if not already running
        if (!isAnimatingScore)
        {
            StartCoroutine(AnimateScore());
        }
    }

    private IEnumerator AnimateScore()
    {
        isAnimatingScore = true;
        
        while (Mathf.Abs(currentDisplayScore - targetScore) > 1)
        {
            // Increment the displayed score toward the target
            currentDisplayScore = Mathf.MoveTowards(
                currentDisplayScore, 
                targetScore, 
                scoreAnimationSpeed * Time.deltaTime
            );
            
            // Update UI with formatted score (always 6 digits)
            if (scoreText != null)
            {
                scoreText.text = $"Score: {currentDisplayScore:000000}";
            }
            
            yield return null;
        }
        
        // Ensure we end on the exact target value
        currentDisplayScore = targetScore;
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentDisplayScore:000000}";
        }
        
        isAnimatingScore = false;
    }

    // Public method to manually update all UI elements
    public void RefreshAllUI()
    {
        if (playerStats != null)
        {
            UpdateHealthUI(playerStats.CurrentHealth, playerStats.MaxHealth);
            UpdateScoreUI(playerStats.Score);
        }
    }

    public void UpdateGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
}
