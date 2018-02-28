using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skateboard : MonoBehaviour
{
    public TrickTextController trickText;
    public ScoreTextController scoreText;

    public TrailRenderer _trailA;
    public TrailRenderer _trailB;
    public OsciloscopeController ZOsciloscope;
    //public OsciloscopeController XOsciloscope;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Rigidbody _rigid;
    private bool _jumping;
    private Vector3 _jumpForce = new Vector3(0, 4.5f, 0);
    private Vector3 _velocity = new Vector3();
    private Vector3 _previousPosition = new Vector3();
    private Vector3 _deltaV;

    // Use this for initialization
    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _startPosition = _rigid.position;
        _startRotation = _rigid.rotation;
    }

    private float modToRotation(float rot)
    {
        return Mathf.Abs(rot % 360f);
    }

    private void FixedUpdate()
    {
        if (!_jumping)
        {
            //Anna hack - if you use arrows on the PC, you can see the skateboard rotating precisely on z or y axis.
            float turn = Input.GetAxis("Horizontal");
            _rigid.AddTorque(transform.forward * 10.0f * turn);
            turn = Input.GetAxis("Vertical");
            _rigid.AddTorque(transform.up * 10.0f * turn);
        }
        _deltaV = transform.position - _previousPosition;
        SetTrail();

        _previousPosition = transform.position;
    }

    private void SetTrail()
    {
        if (_deltaV.y > 0)
        {
            _trailA.time = 0.3f;
            _trailB.time = 0.3f;
        }
        else {
            _trailA.time = 0;
            _trailB.time = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_jumping)
            return;
        _jumping = false;
        Landed();
    }

    private void Landed()
    {
        resetTrickTracking();
    }

    public void doTrick(Vector3 force, Vector3 forcePos, float verticalForce, float trickAngle)
    {
        addForceAtPosition(force, forcePos);
        jump(verticalForce);
        trackTrick(trickAngle);
    }

    public void ResetToStartPosition()
    {
        resetTrickTracking();
        _jumping = false;
        //if you set IsKinematic in one frame, it has zero effect. What you probably want is:
        //_rigid.angularVelocity = Vector3.zero;
        //_rigid.velocity = Vector3.zero;
        _rigid.isKinematic = true;
        _rigid.position = _startPosition;
        _rigid.rotation = _startRotation;
        _rigid.isKinematic = false;
    }

    List<TrickData> trickList = new List<TrickData>() {
        new TrickData("heelflip", 90f, TrickData.TrickAxis.x, 1),
        new TrickData("impossible", 0f, TrickData.TrickAxis.z, 2),
        new TrickData("rest", -999f, TrickData.TrickAxis.z, 0)
    };

    private float _trickAngleTreshold = 30f;

    private void resetTrickTracking()
    {

    }

    public void trackTrick(float angle)
    {
        //track current tricks - dont stack the same kind of tricks, only when a trick has done it's rotation can you add another trick
        //or double the same trick (double kickflip)
        trickText.AddTrick(findTrick(angle).name);
        scoreText.AddScore(findTrick(angle).points);

    }

    private TrickData findTrick(float angle)
    {
        TrickData res = null;

        trickList.ForEach((TrickData t)=>
        {
            if (inRange(t.detectionAngle, angle))
            {
                res = t;
            }
        });

        if(res == null)
        {
            res = trickList[trickList.Count-1];
        }

        return res;
    }

    private bool inRange(float range, float angle)
    {
        if(modToRotation(angle) >= range - _trickAngleTreshold && modToRotation(angle) <= range + _trickAngleTreshold)
        {
            return true;
        }
        return false;
    }

    public void jump(float _force)
    {
        if (_jumping)
            return;
        trickText.ClearText();
        scoreText.ClearText();

        //Assumption that if you start jumping you actually leave the ground is incorrect.
        //on a computer at least, the Box collider attached to the object almost never leaves the ground,
        //therefore _jumping is never reset to false, and the game freezes after the first stunt.
        //the same happens on my mobile
        //move _jumping=true to oncolliderexit
        _jumping = true;
        Debug.Log("FORCE: " + _force);

        Debug.DrawRay(transform.position, Vector3.down, Color.blue, 5f);
        _rigid.AddForceAtPosition(new Vector3(_jumpForce.x, _jumpForce.y * _force, _jumpForce.z), transform.position);
    }

    public void addForceAtPosition(Vector3 _force, Vector3 _position)
    {
        _rigid.AddForceAtPosition(_force, _position);
    }
}