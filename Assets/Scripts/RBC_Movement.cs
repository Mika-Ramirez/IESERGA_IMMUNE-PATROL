using UnityEngine;
using System.Collections.Generic; // Allows us to use Dictionary

public class RBC_Movement : MonoBehaviour
{
    public float moveDistance = 20f; // Distance to move up/down
    private static bool isUp = false; // Shared state across all characters

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // If Space is pressed
        {
            if (isUp)
                transform.position += new Vector3(0, -moveDistance, 0); // Move Down
            else
                transform.position += new Vector3(0, moveDistance, 0); // Move Up

            isUp = !isUp; // Toggle state for ALL characters
        }
    }
}
