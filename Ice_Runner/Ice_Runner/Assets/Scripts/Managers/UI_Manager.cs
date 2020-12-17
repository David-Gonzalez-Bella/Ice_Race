using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    //Variables
    public static UI_Manager sharedInstance { get; private set; } //Singleton
    public float countDownTime;
    public bool countDownActive = true;

    //References
    public GameObject inGameUI;
    public Text lifesText;
    public Text scoreText;
    public Text countDownText;
    public Text levelProgressPercentage;
    public Image levelProgressBar;
    public Slider volumeBar;
    public Button pauseButton;
    [HideInInspector] public Level_Info currentLevel;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Start()
    {
        UpdateScoreText(0);
    }

    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            levelProgressBar.fillAmount = GameManager.sharedInstance.player.transform.position.x / currentLevel.levelSize;
            levelProgressPercentage.text = ((int)(levelProgressBar.fillAmount * 100)).ToString() + " %";

            if (countDownActive)
            {
                if (countDownTime > 0.0f)
                {
                    countDownTime -= Time.deltaTime;
                }
                else
                {
                    countDownTime = 0.0f;
                    countDownActive = false;
                }
                countDownText.text = (Math.Round(countDownTime, 2)).ToString();
            }
        }
    }

    public void UpdateLifesText(Health lifes) //Update UI lifes text
    {
        lifesText.text = "x" + lifes.CurrentLifes;
    }

    public void UpdateScoreText(int quantity)
    {
        scoreText.text = quantity.ToString();
    }
}
