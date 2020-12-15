using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private PlayerController player;
    public int index;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
        {
            if (collision.tag.CompareTo("Player") == 0)
            {
                player = collision.GetComponent<PlayerController>();
                player.currentLevel.currentRespawnPoint = player.currentLevel.respawnPoints[index];

            }
        }
    }
}
