using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform puck;
    public Transform goal;
    public float moveSpeed = 5f;
    public float reactionDelay = 1f;
    public float defenceDistance= 5f;
    public float goalBuffer = 1f;
    public float fieldCenter; 
    public Color gizmoColor = Color.red;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(EnemyUpdate());;
    }

    IEnumerator EnemyUpdate ()
    {
        while (true)
        {
            yield return new WaitForSeconds(reactionDelay);
            bool isPuckInHalf = puck.position.y > fieldCenter;

            if (isPuckInHalf)
            {
                float distanceToPuck = Vector2.Distance(puck.position, transform.position);

                if (distanceToPuck < defenceDistance)
                {
                    Vector2 direction = (puck.position - transform.position).normalized;
                    rb.velocity = direction *moveSpeed;
                }
            }
            else 
            {
                float distanceToGoal = Vector2.Distance(goal.position, transform.position);
                if (distanceToGoal > goalBuffer)
                {
                    Vector2 goalDirection = (goal.position - transform.position).normalized;
                    rb.velocity = goalDirection *moveSpeed;
                }
                else 
                {
                    rb.velocity = Vector2.zero;
                }
            }

                if (rb.velocity.magnitude > moveSpeed)
                {
                rb.velocity = rb.velocity.normalized * moveSpeed;
                }

            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawLine(new Vector3(-10, fieldCenter, 0), new Vector3(10, fieldCenter, 0));
            Gizmos.DrawWireCube(new Vector3(0, (fieldCenter + goal.position.y) / 2, 0), new Vector3(20, Mathf.Abs(fieldCenter - goal.position.y), 0));
        }
}
