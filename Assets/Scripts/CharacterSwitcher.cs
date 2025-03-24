using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject rbc;  // Red Blood Cell
    public GameObject wbc;  // White Blood Cell
    public GameObject platelet;  // Platelet

    private GameObject activeCharacter; // Currently controlled character

    void Start()
    {
        // Set the default character to RBC and disable others
        activeCharacter = rbc;
        activeCharacter.SetActive(true);
        wbc.SetActive(false);
        platelet.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press 1
        {
            SwitchCharacter(rbc);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Press 2
        {
            SwitchCharacter(wbc);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Press 3
        {
            SwitchCharacter(platelet);
        }
    }

    void SwitchCharacter(GameObject newCharacter)
    {
        if (activeCharacter == newCharacter) return; // Prevent switching to the same character

        // Save last position of the currently active character
        Vector3 lastPosition = activeCharacter.transform.position;

        // Disable current character
        activeCharacter.SetActive(false);

        // Enable new character and set its position to the last character's position
        newCharacter.transform.position = lastPosition;
        newCharacter.SetActive(true);

        // Update active character
        activeCharacter = newCharacter;
    }
}
