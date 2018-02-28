using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickData {

    public enum TrickAxis : int { x = 0, z=1 };

    public string name;
    public float detectionAngle;
    public TrickAxis axis;
    public int points;

    public TrickData(string _name, float _detectionAngle, TrickAxis _axis, int _points)
    {
        name = _name;
        detectionAngle = _detectionAngle;
        axis = _axis;
        points = _points;
    }
}
