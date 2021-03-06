﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTriggerWithExit : CameraFollowTrigger
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            if (collision.tag.CompareTo("Player") == 0)
            {
                CameraFollow.sharedInstance.followPlayerY = true;
                CameraFollow.sharedInstance.followOffset = followOffset;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            if (collision.tag.CompareTo("Player") == 0)
                CameraFollow.sharedInstance.followPlayerY = false;
        }
    }
}
