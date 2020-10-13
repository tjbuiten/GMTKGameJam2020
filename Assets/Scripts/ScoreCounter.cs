using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private int scoreAddition = default;
    [SerializeField] private float timeTillAddition = default;
    private TextMeshProUGUI scoreText = default;
    private string defaultText = "Score: ";
    private int score = 0;
    private IEnumerator scoreAdditionCoroutine = null;

    public ArrayList scores = new ArrayList();

    private bool updatingScore = false;

    private void Awake()
    {
        scoreAdditionCoroutine = ScoreAddition();
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
        scoreText.SetText(defaultText + score);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (updatingScore)
        {
            scoreText.SetText(defaultText + score);
        }
    }

    private IEnumerator ScoreAddition()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(timeTillAddition);
            score += scoreAddition;
        }
    }

    public void AddToScore(int bonus)
    {
        score += bonus;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void StartAddingToScore()
    {
        updatingScore = true;
        StartCoroutine(scoreAdditionCoroutine);
    }

    public void UpdateScores()
    {
        FindObjectOfType<ScoreManager>().addScore(score);
    }

    public void StopAddingToScore()
    {
        updatingScore = false;
        StopCoroutine(scoreAdditionCoroutine);
    }
}
