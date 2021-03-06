﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour, IScreen
{
    private GameController _game;

    public string routeName
    {
        get
        {
            return "Score";
        }
    }

    public void init(GameController game)
    {
        _game = game;
        enableMe();
    }

    public void enableMe()
    {
        gameObject.SetActive(true);
    }

    public void disableMe()
    {
        gameObject.SetActive(false);
    }

    public void onStartBtn()
    {
        _game.resolveRoute("Game");
    }

}
