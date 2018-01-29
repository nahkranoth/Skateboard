using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OsciloscopeController : MonoBehaviour {

    public RawImage image;
    public Color backgroundColor;

    private float _val;

    void Start()
    {
        image.material.SetColor("_BGColor", backgroundColor);
    }

    public void SetNormalizedValue(float val)
    {
        _val = Mathf.Clamp(val, 0f, 1f);
    }

	// Update is called once per frame
	void FixedUpdate () {
        //_val = (Mathf.Sin(Time.time) * 0.5f)+0.5f;
        //Debug.Log(_val);
        image.material.SetFloat("_Value", _val);
    }
}
