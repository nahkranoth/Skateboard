using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrickTextController : MonoBehaviour {

    private string _trickText;
    private Text textUI;
    private Color originalColor;
    private int originalSize;

    private int fontGrowSize = 12;

    void Start()
    {
        textUI = GetComponent<Text>();
        originalColor = textUI.color;
        originalSize = textUI.fontSize;

        if (!textUI)
        {
            Debug.LogError("No Text Component on this GameObject");
        }
    }

    public void fireCompleted()
    {
        textUI.color = Color.green;
        textUI.fontSize = textUI.fontSize + fontGrowSize;
        StartCoroutine(resetFireCompletedColor());
        StartCoroutine(resetFireGrowSize());
    }

    private IEnumerator resetFireGrowSize()
    {
        yield return new WaitForSeconds(0.3f);
        textUI.fontSize = originalSize;
    }

    private IEnumerator resetFireCompletedColor()
    {
        yield return new WaitForSeconds(1f);
        textUI.color = originalColor;
    }

    public void setScoreFail()
    {
        textUI.color = Color.red;
        StopAllCoroutines();
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
        textUI.color = originalColor;
        textUI.fontSize = originalSize;
        textUI.text = "";
        _trickText = "";
        StopAllCoroutines();
    }

}
