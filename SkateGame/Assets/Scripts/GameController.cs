using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public AudioManager audioManager;
    public GameObject homeScreen;
    public GameObject scoreScreen;
    public GameObject dummyScreen;
    public Skateboard skateBoard;
    public Text timerText;
    public Text endScoreText;
    public TotalScore totalScoreText;

    private IScreen home;
    private IScreen score;
    private IScreen dummy;

    private IScreen activeScreen;

    private List<IScreen> screenList;

    private DateTime startTime;
    private bool timerToggled;


    // Use this for initialization
    void Start () {
        home = resolveScreen(homeScreen);
        home.init(this);
        home.enableMe();
        activeScreen = home;//cleanup

        score = resolveScreen(scoreScreen);
        score.init(this);
        score.disableMe();

        dummy = resolveScreen(dummyScreen);
        dummy.init(this);

        screenList = new List<IScreen>() { home, score, dummy };
    }

    private IScreen resolveScreen(GameObject screen)
    {
        return screen.GetComponent<IScreen>();
    }

    public void resolveRoute(string route)
    {
        if(route == "Game")
        {
            totalScoreText.resetScoreText();
            home.disableMe();
            switchScreen(dummy);
            startGame();
            return;
        }
        screenList.ForEach((screen) => {
            if(screen.routeName == route)
            {
                switchScreen(screen);
            }
        });
    }

    public void switchScreen(IScreen screen)
    {
        activeScreen.disableMe();
        screen.enableMe();
        activeScreen = screen;
    }

    private TimeSpan roundTime = new TimeSpan(0, 1, 0);

    void Update()
    {
        if (timerToggled) { 
            DateTime now = DateTime.Now;
            TimeSpan delta = roundTime - (now - startTime);
            timerText.text = String.Format("{0} : {1}",delta.Minutes, delta.Seconds);
            if (delta <= TimeSpan.Zero) endGame();
        }
    }

    private void startGame()
    {
        startTime = DateTime.Now;
        skateBoard.activateMe();
        //audioManager.start();
        timerToggled = true;
    }

    private void endGame()
    {
        endScoreText.text = totalScoreText.score.ToString();
        timerToggled = false;
        switchScreen(score);
        audioManager.stop();
    }
}
