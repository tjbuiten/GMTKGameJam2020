using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public ArrayList scores = new ArrayList();
    public int lastScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addScore(int score)
    {
        int i = scores.Count;

        for (int j = scores.Count; j > 0; j--)
        {
            if (score < (int)scores[j - 1])
            {
                i = j;
                break;
            }

            if (j == 1)
                i = 0;
        }

        scores.Insert(i, score);
        lastScore = score;
    }
}
