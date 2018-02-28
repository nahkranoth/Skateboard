using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextController : MonoBehaviour {

    private string _trickText;
    private Text textUI;

    private int currentScore = 0;

    void Start()
    {
        textUI = GetComponent<Text>();
        if (!textUI)
        {
            Debug.LogError("No Text Component on this GameObject");
        }
    }

    public void AddScore(int _score)
    {
        currentScore += _score;
        _trickText = currentScore.ToString();
        SetText();
    }

    private void SetText()
    {
        textUI.text = _trickText;
    }

    public void ClearText()
    {
        currentScore = 0;
        textUI.text = "";
        _trickText = "";
    }

	// Update is called once per frame
	void Update () {
		
	}
}
