using UnityEngine;
using System.Collections;

/// <summary>
/// From: https://forum.unity.com/threads/counting-an-objects-rotations.4170/
/// </summary>
public class CountRotations : MonoBehaviour
{
    private float totalRotation = 0;

    public int nrOfRotations
    {
        get
        {
            return ((int)totalRotation) / 360;
        }
    }

    private Vector3 lastPoint;

    // Use this for initialization
    private void Start()
    {
        //Forward and Y to correctly predict rotation around y axis
        //up and x to correctly predict rotation around x axis
        //right and z to correctly predict rotation around z axis
        lastPoint = transform.TransformDirection(Vector3.up);
        lastPoint.x = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 facing = transform.TransformDirection(Vector3.up);
        facing.x = 0;

        float angle = Vector3.Angle(lastPoint, facing);
        if (Vector3.Cross(lastPoint, facing).x < 0)
            angle *= -1;

        totalRotation += angle;
        lastPoint = facing;
    }
}