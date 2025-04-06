using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoteDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerStats playerStats;
    void Start()
    {
        if(playerStats == null)
        {
            playerStats = FindFirstObjectByType<PlayerStats>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            Note noteScript = collision.GetComponent<Note>();

            if (noteScript != null)
            {
                noteScript.AnimateAndDestroy();
                GameManager.Instance.DisplayFeedback("Miss");
            }
            playerStats.TakeDamage(30);
            Destroy(collision.gameObject);
        }
    }
}
