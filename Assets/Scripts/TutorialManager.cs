using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> tutorialSteps;
    [SerializeField]private GameObject continuePrompt; // Assign a "Press Space to Continue" UI element
    [SerializeField]private SpriteRenderer fadeSprite;
    
    private float fadeDuration = 1f; // Duration of fade in seconds
    private float initialDelay = 3.0f; // Time to show step before prompt appears
    private int currentStepIndex = 0;
    private bool awaitingInput = false;

    private void Start()
    {
        if(fadeSprite != null)
        {
            Color color = fadeSprite.color;
            color.a = 0f;
            fadeSprite.color = color;
            StartCoroutine(FadeIn(fadeSprite));
        }    
        // Make sure the continue prompt is hidden initially
        if (continuePrompt != null)
            continuePrompt.SetActive(false);

        foreach(GameObject step in tutorialSteps)
        {
            step.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for spacebar press to advance tutorial
        if (awaitingInput && Input.GetKeyDown(KeyCode.Space) && currentStepIndex <= tutorialSteps.Count)
        {
            Debug.Log("going to next step");
            awaitingInput = false;
            continuePrompt.SetActive(false);
            
            // Hide current step
            if (currentStepIndex > 0 && currentStepIndex <= tutorialSteps.Count)
            {
                tutorialSteps[currentStepIndex - 1].SetActive(false);
            }
            
            // Move to next step
            if (currentStepIndex < tutorialSteps.Count)
            {
                ShowNextStep();
            }
            else
            {
                // Tutorial complete
                FinishTutorial();
            }
        }
    }

    private IEnumerator FadeIn(SpriteRenderer fadeRenderer)
    {
        Color color = fadeRenderer.color;
        float targetAlpha = 170f / 255f;
        float elapsedTime = 0f;
        float startAlpha = color.a;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration; // Value between 0 and 1
            color.a = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
            fadeRenderer.color = color;
            yield return null;
        }

        // Ensure we end exactly at the target alpha
        color.a = targetAlpha;
        fadeRenderer.color = color;

        if (tutorialSteps.Count > 0)
        {
            ShowNextStep();
        }
    }

    private IEnumerator FadeOut(SpriteRenderer fadeRenderer)
    {
        Color color = fadeRenderer.color;
        float targetAlpha = 0f;
        float elapsedTime = 0f;
        float startAlpha = color.a;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration; // Value between 0 and 1
            color.a = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
            fadeRenderer.color = color;
            yield return null;
        }

        // Ensure we end exactly at the target alpha
        color.a = targetAlpha;
        fadeRenderer.color = color;

        if(currentStepIndex >= tutorialSteps.Count)
        {
            StartCoroutine(GameManager.Instance.StartCountdown());
        }
    }

    private void ShowNextStep()
    {
        tutorialSteps[currentStepIndex].SetActive(true);
        currentStepIndex++;
        
        if (currentStepIndex == tutorialSteps.Count)
            continuePrompt.GetComponent<TMP_Text>().text = "Press Space to Start Game!";
        // Start the countdown to show the continue prompts
        StartCoroutine(WaitForContinuePrompt());
    }

    private IEnumerator WaitForContinuePrompt()
    {
        // Wait for the initial delay
        yield return new WaitForSeconds(initialDelay);
        
        // Show the continue prompt
        if (continuePrompt != null)
            continuePrompt.SetActive(true);
            
        // Set flag to check for input
        awaitingInput = true;
    }
    
    private void FinishTutorial()
    {
        // Optional: Add any code to run when the tutorial is complete
        Debug.Log("Tutorial completed!");
        
        StartCoroutine(FadeOut(fadeSprite));
        // You might want to destroy the tutorial or transition to gameplay
        // gameObject.SetActive(false);
    }
}