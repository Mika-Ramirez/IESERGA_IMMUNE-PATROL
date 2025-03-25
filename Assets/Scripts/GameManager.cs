using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private AudioSource music;
    private bool hasStarted = false;
    public static event Action OnGameStart; // Event for game start
    private List<Note> activeNotes  = new List<Note>();
    private float hp = 100;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        if (music == null)
            music = GetComponent<AudioSource>(); // Ensure music is assigned
    }

    void Update()
    {
        if (!hasStarted && Input.anyKeyDown)
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        hasStarted = true;
        music.Play();

        // Wait until the music starts playing
        yield return new WaitUntil(() => music.isPlaying);

        OnGameStart?.Invoke(); 
    }

    public void RegisterNote(Note note)
    {
        activeNotes.Add(note); 
    }

    public void RemoveNote(Note note)
    {
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
            //Win
            Debug.Log("WinGame");
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(3f);
        CheckForAllFinishedNotes();
    }

    private void WinGame()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
