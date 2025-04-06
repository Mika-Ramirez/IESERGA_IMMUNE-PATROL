using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenPickup : MonoBehaviour
{
    public int oxygenValue = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Activator")) // Make sure your RBC has tag "Player"
        {
            // Find UI Manager and update
            FindObjectOfType<OxygenUI>().AddOxygen(oxygenValue);

            // OPTIONAL: play sound or animation

            Destroy(gameObject); // Remove oxygen after pickup
        }
    }
}
