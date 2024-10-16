using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Basic enemy, travels left, right, up, or down depending on where it spawns compared to the player
public class EnemyControllerBasic : Enemy
{
    //

    override protected void Start()
    {
        base.Start();

        if (lookTarget != null)
        {
            float deltaX = startPosition.x - lookTarget.position.x;
            float deltaY = startPosition.y - lookTarget.position.y;

            // Determine movement direction by comparing relation to target
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
            {
                // Horizontal movement
                if (deltaX < 0)
                {
                    // enemy spawned to left of player
                    movementDirection = Vector3.right;
                }
                else
                {
                    // enemy spawned to right of player
                    movementDirection = Vector3.left;
                }
            }
            else
            {
                // Vertical movement
                if (deltaY < 0)
                {
                    // enemy spawned below player
                    movementDirection = Vector3.up;
                }
                else
                {
                    movementDirection = Vector3.down;
                }
            }
        }
    }
}
