using System;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [SerializeField] private float beatsPerMinute;
    private float beatSpeed;
    private bool hasStarted = false;

    void Start()
    {
        beatSpeed = beatsPerMinute / 60f;
        GameManager.OnGameStart += StartScrolling; // Subscribe to GameManager event
    }

    void Update()
    {
        if (hasStarted)
        {
            MoveBeats();
        }
    }

    private void MoveBeats()
    {
        transform.position -= new Vector3(beatSpeed * Time.deltaTime, 0f, 0f);
    }

    private void StartScrolling()
    {
        hasStarted = true;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= StartScrolling; // Unsubscribe to avoid memory leaks
    }
}
