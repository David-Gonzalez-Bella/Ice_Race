using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTrigger : MonoBehaviour
{
    public float followOffset;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            if (collision.tag.CompareTo("Player") == 0) //If the player exits the dead zone
            {
                CameraFollow.sharedInstance.followPlayerY = !CameraFollow.sharedInstance.followPlayerY;
                CameraFollow.sharedInstance.followOffset = followOffset;
            }
        }
    }
}
