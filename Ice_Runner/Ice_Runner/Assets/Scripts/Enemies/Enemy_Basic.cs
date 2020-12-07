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
        if (rb.transform.localPosition.x >= endMovePoint)
        {
            direction = -1;
            sr.flipX = true;
        }
        else if (rb.transform.localPosition.x <= startMovePoint)
        {
            direction = 1;
            sr.flipX = false;
        }
    }
}
