using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Basic : Enemy
{
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xSpeed * direction, rb.velocity.y);
    }

    protected override void Behaviour()
    {
        if (rb.position.x >= endMovePoint)
        {
            direction = -1;
            sr.flipX = true;
        }
        else if (rb.position.x <= startMovePoint)
        {
            direction = 1;
            sr.flipX = false;
        }
    }
}
