using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    public static OxygenManager instance;
    public int oxygenCount = 0; // Stores collected oxygen
    public Text oxygenText; // Reference to UI text

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void AddOxygen()
    {
        oxygenCount++;
        oxygenText.text = "Oxygen: " + oxygenCount; // Update UI
    }
}
