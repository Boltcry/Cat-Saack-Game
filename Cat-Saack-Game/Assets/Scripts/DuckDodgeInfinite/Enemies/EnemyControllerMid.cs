using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Medium enemy, moves in a straight direction towards the player's position upon spawn
public class EnemyControllerMid : Enemy
{
    //

    override protected void Start()
    {
        base.Start();

        if (lookTarget != null)
        {
            Vector3 lookDirection = lookTarget.position - transform.position;
            movementDirection = lookDirection.normalized;
            LookToDirection(movementDirection);
        }
    }
}
