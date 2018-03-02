using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour {

    public Text scoreText;
    private int score;

	// Use this for initialization
	void Start () {
        resetScoreText();
    }

    void setText()
    {
        scoreText.text = score.ToString();
    }

    public void setScoreText(int _score)
    {
        score += _score;
        setText();
    }

    public void resetScoreText()
    {
        score = 0;
        setText();
    }
}
