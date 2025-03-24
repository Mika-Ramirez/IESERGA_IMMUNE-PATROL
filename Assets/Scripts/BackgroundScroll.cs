using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 2f;  // Speed of scrolling
    public float backgroundWidth;   // Set this to your background width

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Save the starting position
    }

    void Update()
    {
        // Move the background to the left
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // If the background has moved out of view, reposition it to the right
        if (transform.position.x <= startPosition.x - backgroundWidth)
        {
            transform.position += new Vector3(backgroundWidth * 2, 0, 0);
        }
    }
}
