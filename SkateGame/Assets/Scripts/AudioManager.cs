using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource song;
    public AudioSource jump;
    public AudioSource land;
    public AudioSource fail;

    public void start()
    {
        song.Play();
    }

    public void stop()
    {
        song.Stop();
    }
	
    public void playJump()
    {
        jump.Play();
    }

    public void playLanded()
    {
        land.Play();
    }

    public void playFail()
    {
        fail.Play();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
