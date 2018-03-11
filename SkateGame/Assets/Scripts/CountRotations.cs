using UnityEngine;
using System.Collections;

/// <summary>
/// From: https://forum.unity.com/threads/counting-an-objects-rotations.4170/
/// </summary>
public class CountRotations : MonoBehaviour
{
    private float totalXRotation = 0;
    private float totalZRotation = 0;

    public OsciloscopeController zOsc;

    public int completedXRotations = 0;
    public int completedZRotations = 0;

    public int nrOfXRotations
    {
        get
        {
            return ((int)totalXRotation) / 360;
        }
    }

    public int nrOfZRotations
    {
        get
        {
            return ((int)totalZRotation) / 360;
        }
    }

    private Vector3 lastXPoint;
    private Vector3 lastZPoint;

    // Use this for initialization
    private void Start()
    {
        //Forward and Y to correctly predict rotation around y axis
        //up and x to correctly predict rotation around x axis
        //right and z to correctly predict rotation around z axis
        Reset();

    }

    //I need to check against the Trick i'm expecting.
    //A kickflip checks over axis z
    //Impossible checks over axis x
    //Vector3.forward for NO x axis
    //Vector3.back for No z axis

    // Update is called once per frame

    private void updateXRotation()
    {
        Vector3 facing = transform.TransformDirection(Vector3.back);
        facing.x = 0;

        float angle = Vector3.Angle(lastXPoint, facing);

        if (Vector3.Cross(lastXPoint, facing).x < 0)
            angle *= -1;


        totalXRotation += angle;
        zOsc.SetNormalizedValue(totalXRotation / 360f);

        lastXPoint = facing;
    }

    private void updateZRotation()
    {
        Vector3 facing = transform.TransformDirection(Vector3.left);
        facing.z = 0;

        float angle = Vector3.Angle(lastZPoint, facing);

        if (Vector3.Cross(lastZPoint, facing).x < 0)
            angle *= -1;

        totalZRotation += angle;

        lastZPoint = facing;
    }

    public void Reset()
    {
        lastXPoint = transform.TransformDirection(Vector3.back);
        lastXPoint.x = 0;
        lastZPoint = transform.TransformDirection(Vector3.left);
        lastZPoint.z = 0;
        totalXRotation = 0;
        totalZRotation = 0;
        completedXRotations = 0;
        completedZRotations = 0;
    }


    private void FixedUpdate()
    {
        //Unity rotation order:  Z-X-Y
        updateZRotation();
        updateXRotation();

        zOsc.SetNormalizedValue(totalZRotation / 360f);
        //nr of rotations is fragile because we may rotate back a little, better to use the current maximum amount:
        if (Mathf.Abs(nrOfXRotations) > completedXRotations)
        {
            completedXRotations = Mathf.Abs(nrOfXRotations);
        }
        if (Mathf.Abs(nrOfZRotations) > completedZRotations)
        {
            completedZRotations = Mathf.Abs(nrOfZRotations);
        }
    }
}