using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Advanced enemy, will travel towards the player in wide arcs for a certain amount of time before traveling in a straight line off the screen
public class EnemyControllerAdv : Enemy
{
    [Tooltip("Amount of time in seconds the enemy will follow the target")]
    public float followTime = 10f;
    public float arcRadius = 3f;
    public float arcSpeed = 2f;

    private float arcAngle;

    private float timeSinceSpawn = 0f;

    override protected void Update()
    {
        if (MinigameManagerDuck.gameIsRunning)
        {
            timeSinceSpawn += Time.deltaTime;

            if (timeSinceSpawn <= followTime)
            {
                arcAngle += arcSpeed * Time.deltaTime;
                Vector3 offset = new Vector3(Mathf.Cos(arcAngle), Mathf.Sin(arcAngle), 0) * arcRadius;

                movementDirection = offset.normalized;
                LookToDirection(movementDirection);
            }

            base.Update();
        }
    }
}
