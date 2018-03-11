using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{

	public Skateboard skateboard;
	public Transform myCamera;

	private Vector3 _prevMousePos = new Vector3 ();
	private Vector3 _startDragMousePos = new Vector3 ();
	private Vector3 _mousePos = new Vector3 ();
	private Vector3 _deltaMousePos = new Vector3 ();
	private Vector3 _deltaDrag = new Vector3 ();

	private float _moveMagnitude;
	private const float _contactTreshold = 0.2f;
	private bool _contactFireToggle = false;
	private System.Diagnostics.Stopwatch _runTimer;

	private const float _contactForce = 100f;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton (0)) {
			InputController ();
		} else {
			ResetInputController ();
		}
	}

	private bool _mouseClickToggle = true;

	void ResetInputController ()
	{
		if (!_mouseClickToggle)
			EndTrajectory ();
		_mouseClickToggle = true;
	}

	void InputController ()
	{
		if (_mouseClickToggle) {
			//reset press
			_mouseClickToggle = false;
			ResetTrajectory ();
		}

		//Note! Order is important for input of mouse positions into methods
		_mousePos = getNormalizedMousePos ();
		_deltaMousePos = _mousePos - _prevMousePos;
		_deltaDrag = _mousePos - _startDragMousePos;
		_moveMagnitude = _deltaMousePos.magnitude;
		ObserveTrajectory ();
		_prevMousePos = _mousePos;
	}

	void EndTrajectory ()
	{
		//Debug.Log("DETLA DRAG: " + _deltaDrag);
		//Debug.Log("GET: "+EventController.Get());
	}

	Vector3 getNormalizedMousePos ()
	{
		return new Vector3 (Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, 0);
	}

	void ResetTrajectory ()
	{
		_prevMousePos = getNormalizedMousePos ();
		_startDragMousePos = getNormalizedMousePos ();
		_runTimer = new System.Diagnostics.Stopwatch ();
		_runTimer.Start ();
		_contactFireToggle = false;
	}

	void ObserveTrajectory ()
	{
		float _cutoff = 1f;
		Debug.DrawLine (_prevMousePos, _mousePos, Color.red, 5f);
		float _cappedDragMagnitude = Mathf.Min (_deltaDrag.magnitude, _cutoff);

		if (_cappedDragMagnitude >= _contactTreshold && !_contactFireToggle) {
			_contactFireToggle = true;
			FireContactThreshold ();
		}
	}

	private const float _forwardPositionWeight = .622f;
	private const float _forceMagnitude = 20f;

	void FireContactThreshold ()
	{
		_runTimer.Stop ();
		float _elapsedTime = _runTimer.ElapsedMilliseconds;
		float _contactFireForce = _contactForce / _elapsedTime;

		float _dragAngle = Mathf.Atan2 (
			                         getNormalizedMousePos ().x - _startDragMousePos.x, 
			                         getNormalizedMousePos ().y - _startDragMousePos.y
		                         ) * (180 / Mathf.PI);

		int _direction = _dragAngle > 0 ? 1 : -1;
		float _angleToZPosition = _forwardPositionWeight - (_forwardPositionWeight * (Mathf.Abs (_dragAngle) / 180f)) * 2f;
		float _xPolarIntensity = GetXIntesity (_dragAngle);

		Vector3 _rotatedPosition = Quaternion.Euler (0, skateboard.transform.eulerAngles.y, 0) * new Vector3 (_xPolarIntensity * 0.05f, 0.3f, _angleToZPosition);
		Vector3 _forcePosition = _rotatedPosition + skateboard.transform.position;
		Vector3 _forceVector = skateboard.transform.position + new Vector3 (0, -_contactFireForce * _forceMagnitude, 0);

		Debug.DrawRay (_forcePosition, Vector3.down, Color.green, 5f);

        Vector3 _force = new Vector3(0, -_contactFireForce * _forceMagnitude, 0);

        skateboard.doTrick(_force, _forcePosition, _contactFireForce * _forceMagnitude, _dragAngle);
	}

	//Here we divide the angle of forward drag to 5 steps
	float GetXIntesity (float angle)
	{
		float _normalizedAngle = angle / 18;//as in -180 to 180 (divided by 10 to normalize)
		float _cappedAngle = Mathf.Clamp (_normalizedAngle, -5, 5);
		return _cappedAngle;
	}
}
