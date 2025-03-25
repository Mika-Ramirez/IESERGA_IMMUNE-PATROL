using System;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] private GameObject note;
    [SerializeField] private bool createMode;
    [SerializeField] private GameObject noteHolder;

    void Update()
    {
        if (createMode && Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newNote = Instantiate(note, transform.position, Quaternion.identity);
            newNote.transform.SetParent(noteHolder.transform); // Set parent to noteHolder
        }
    }
}
