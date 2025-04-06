using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private UnityEngine.UI.Image background;
     // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        if(GameManager.Instance.IsPlayerDead)
        {
            background.color = loseColor;
            statusText.text = "Your Body Died";
        }
        else
        {
            background.color = winColor;
            statusText.text = "Your Body is Healthy";
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
