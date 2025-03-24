using UnityEngine;

public class OxygenCollect : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if RBC touches it
        {
            Destroy(gameObject); // Remove oxygen
            OxygenManager.instance.AddOxygen(); // Update oxygen count
        }
    }
}
