using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skateboard : MonoBehaviour
{
    public TrickTextController trickText;
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

    private void FixedUpdate()
    {
        if (_jumping)
        {
            //Debug.Log(transform.forward - (transform.eulerAngles/360));//.z is z axis normalized
            //Debug.Log(transform.right);

            ZOsciloscope.SetNormalizedValue(transform.eulerAngles.z / 360);

            //ZOsciloscope.SetNormalizedValue(transform.eulerAngles.y / 360);
            //XOsciloscope.SetNormalizedValue(transform.eulerAngles.x / 360);

            TrackRotation(transform.localEulerAngles.x);
        }
        else
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

    private bool _halfFlipZTrigger = false;
    private bool _fullFlipZTrigger = false;

    private void TrackRotation(float rotation)
    {
        //Debug.Log(rotation);
        //I'm not sure if it would not be better to add current and previous rotations, track it that way.
        if (InRange(Mathf.Abs(rotation), 160, 180) && !_halfFlipZTrigger)
        {
            _halfFlipZTrigger = true;
            Debug.Log("Half flip");
        }
        if (InRange(Mathf.Abs(rotation), 0, 20) && _halfFlipZTrigger)
        {
            _halfFlipZTrigger = false;
            _fullFlipZTrigger = true;
            Debug.Log("Full flip");
        }
    }

    private bool InRange(float val, float min, float max)
    {
        if (val <= max && val >= min)
        {
            return true;
        }
        return false;
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
        if (_fullFlipZTrigger)
        {
            trickText.AddTrick("Full Flip");
        }
        ResetFlipTracking();
    }

    private void ResetFlipTracking()
    {
        _halfFlipZTrigger = false;
        _fullFlipZTrigger = false;
    }

    public void ResetToStartPosition()
    {
        ResetFlipTracking();
        _jumping = false;
        //if you set IsKinematic in one frame, it has zero effect. What you probably want is:
        //_rigid.angularVelocity = Vector3.zero;
        //_rigid.velocity = Vector3.zero;
        _rigid.isKinematic = true;
        _rigid.position = _startPosition;
        _rigid.rotation = _startRotation;
        _rigid.isKinematic = false;
    }

    public void Jump(float _force)
    {
        if (_jumping)
            return;
        trickText.ClearText();
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

    public void AddForceAtPosition(Vector3 _force, Vector3 _position)
    {
        _rigid.AddForceAtPosition(_force, _position);
    }
}