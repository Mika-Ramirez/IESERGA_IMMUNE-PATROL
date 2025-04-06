using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

// PlayerStats.cs - Manages the player's statistics
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxScore = 1000000; // Maximum score for all perfects
    [SerializeField] private float score = 0;
    
    // Scoring multipliers
    private const float PERFECT_MULTIPLIER = 1.0f;     // 100% of perfect points
    private const float GREAT_MULTIPLIER = 0.8f;       // 80% of perfect points
    private const float GOOD_MULTIPLIER = 0.6f;        // 60% of perfect points
    public float pointsPerNote;
    
    // Track note hit accuracy counts
    private int perfectHits = 0;
    private int greatHits = 0;
    private int goodHits = 0;

    // Events that other classes can subscribe to
    public event Action<float, float> OnHealthChanged; // Current, Max
    public event Action<float> OnScoreChanged;
    public event Action<float> OnLevelChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Notify UI of initial values
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnScoreChanged?.Invoke(score);
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // This method calculates points based on accuracy and total notes
    public void AddNoteScore(string accuracy)
    {
        Debug.Log("Points per note:" + pointsPerNote);
        int pointsEarned = 0;
        
        switch (accuracy)
        {
            case "Perfect!":
                pointsEarned = Mathf.RoundToInt(pointsPerNote * PERFECT_MULTIPLIER);
                perfectHits++;
                break;
            case "Great!":
                pointsEarned = Mathf.RoundToInt(pointsPerNote * GREAT_MULTIPLIER);
                greatHits++;
                break;
            case "Good!":
                pointsEarned = Mathf.RoundToInt(pointsPerNote * GOOD_MULTIPLIER);
                goodHits++;
                break;
            default:
                // Miss or other cases
                pointsEarned = 0;
                break;
        }
        
        score += pointsEarned;
        OnScoreChanged?.Invoke(score);
    }

    private void Die()
    {
        // Handle player death logic
        Debug.Log("Player died!");
        GameManager.Instance.LoseGame();
        // Notify GameManager or trigger game over sequence
        //GameManager.Instance.PlayerDied();
    }

    // Get current hit statistics
    public int GetPerfectHits() => perfectHits;
    public int GetGreatHits() => greatHits;
    public int GetGoodHits() => goodHits;
    
    // Reset hit counters for new level
    public void ResetHitCounters()
    {
        perfectHits = 0;
        greatHits = 0;
        goodHits = 0;
    }

    // Getters for current values
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float Score => score;
    public float MaxScore => maxScore;
}