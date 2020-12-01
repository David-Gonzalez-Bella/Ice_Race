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
    private bool countDownActive = true;

    //References
    public Text lifesText;
    public Text scoreText;
    public Text countDownText;
    public Text levelProgressPercentage;
    public Image levelProgressBar;
    private Level_Info currentLevel;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Start()
    {
        currentLevel = GameObject.FindGameObjectWithTag("Level").GetComponent<Level_Info>();
        UpdateScoreText(0);
    }

    void Update()
    {
        levelProgressBar.fillAmount = GameManager.sharedInstance.player.transform.position.x / currentLevel.levelSize;
        levelProgressPercentage.text = ((int)(levelProgressBar.fillAmount * 100)).ToString() + " %";

        if(countDownActive)
        {
            if(countDownTime > 0.0f)
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

    public void UpdateLifesText(Health lifes) //Update UI lifes text
    {
        lifesText.text = lifes.CurrentLifes + " x " + lifes.maxLifes;
    }

    public void UpdateScoreText(int quantity)
    {
        scoreText.text = quantity.ToString();
    }
}
