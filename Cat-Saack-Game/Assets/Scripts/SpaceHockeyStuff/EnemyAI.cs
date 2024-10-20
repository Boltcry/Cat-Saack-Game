using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform puck;
    public Transform goal;
    public float moveSpeed = 5f;
    public float reactionDelay = 0.2f; 
    public float defendDistance = 3f; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AIUpdate());
    }

    IEnumerator AIUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(reactionDelay);

            
            float distanceToPuck = Vector2.Distance(puck.position, transform.position);
            float distanceToGoal = Vector2.Distance(goal.position, transform.position); // find puck

            

            if (distanceToPuck < defendDistance && distanceToGoal > defendDistance)
            {
                Vector2 direction = (puck.position - transform.position).normalized;////// defend against puck
                rb.velocity = direction * moveSpeed;
            }
            else
            {
                
                Vector2 directionToGoal = (goal.position - transform.position).normalized; ///Defense with no puck
                rb.velocity = directionToGoal * moveSpeed;
            }

            
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
    }
}
