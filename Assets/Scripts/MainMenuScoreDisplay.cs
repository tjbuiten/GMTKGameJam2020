using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuScoreDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        int lastScore = 0;

        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (sm != null)
            lastScore = sm.lastScore;

        text.SetText("Last score: " + lastScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
