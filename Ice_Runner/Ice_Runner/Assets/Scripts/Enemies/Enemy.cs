using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Health))]

public class Enemy : MonoBehaviour
{
    //Variables
    //Movement
    protected int direction = 1;
    public float xSpeed;
    public float startMovePoint;
    public float endMovePoint;

    //References
    protected Rigidbody2D rb;
    [HideInInspector]public SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == gameState.inGame)
            Behaviour(); //All enemies will do its behaviour every frame
    }

    protected virtual void Behaviour() { } //Each enemy has a different behaviour
}
