using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickTextController : MonoBehaviour {

    private string _trickText;
    private Text textUI;

    void Start()
    {
        textUI = GetComponent<Text>();
        if (!textUI)
        {
            Debug.LogError("No Text Component on this GameObject");
        }
    }

    public void AddTrick(string _txt)
    {
        string _suffix = "";

        if (textUI.text != "") _suffix = " + ";

        _trickText += _suffix + _txt;
        SetText();
    }

    private void SetText()
    {
        textUI.text = _trickText;
    }

    public void ClearText()
    {
        textUI.text = "";
        _trickText = "";
    }

	// Update is called once per frame
	void Update () {
		
	}
}
