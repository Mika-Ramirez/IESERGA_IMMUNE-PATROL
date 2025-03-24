using UnityEngine;

public class CheckBackgroundSize : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            float width = sr.bounds.size.x;
            Debug.Log("Background Width: " + width);
        }
        else
        {
            Debug.Log("No Sprite Renderer found!");
        }
    }
}
