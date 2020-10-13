using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopScores : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScoreManager scoreCounter = FindObjectOfType<ScoreManager>();

        if (scoreCounter == null)
            return;

        object[] allScores = scoreCounter.scores.ToArray();

        string scores = "";

        for (int i = 0; i < 10; i++)
        {
            if (i >= allScores.Length)
                break;

            scores += "#" + i + " " + allScores[i] + "\n";
        }

        GetComponent<TextMeshProUGUI>().SetText(scores);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
