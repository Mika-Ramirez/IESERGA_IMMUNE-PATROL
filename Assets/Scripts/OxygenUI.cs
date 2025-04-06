using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OxygenUI : MonoBehaviour
{
    public TextMeshProUGUI oxygenText;
    private int oxygenCount = 0;

    public void AddOxygen(int amount)
    {
        oxygenCount += amount;
        oxygenText.text = "Oxygen: " + oxygenCount;
    }
}
