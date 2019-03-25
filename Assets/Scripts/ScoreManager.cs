using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    private int score = 0;
    private int high_score = 0;
    private TextMesh score_label;
    private TextMesh high_score_label;

    public ScoreManager(TextMesh score_label, TextMesh high_score_label)
    {
        this.score_label = score_label;
        this.high_score_label = high_score_label;
        SetHighScore(PlayerPrefs.GetInt("high_score", 0));
    }

    public void Add()
    {
        score++;
        score_label.text = score.ToString();
        if(score > high_score)
        {
            SetHighScore(score);
        }
    }

    void SetHighScore(int score)
    {
        high_score = score;
        high_score_label.text = high_score.ToString();
        PlayerPrefs.SetInt("high_score", score);
    }

    public void Reset()
    {
        score = 0;
        score_label.text = score.ToString();
    }
}
