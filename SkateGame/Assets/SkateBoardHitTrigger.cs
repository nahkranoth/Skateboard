using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateBoardHitTrigger : MonoBehaviour {

    public bool active;

    private void OnTriggerEnter(Collider other)
    {
        active = true;
    }

    private void OnTriggerExit(Collider other)
    {
        active = false;
    }
}
