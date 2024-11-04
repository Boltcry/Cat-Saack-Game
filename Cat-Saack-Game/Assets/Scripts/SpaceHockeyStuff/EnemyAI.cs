using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform goal;
    public Transform puck;
    public float moveSpeed = 5f;
    public float guardSpace = 3f;
    public float frontSpace = 1f;
    public float waitTime = 0.5f;
    public float laserSpeed = 10f;
    public float shootInterval = 3f;
    public GameObject laserPrefab;
    private Vector3 leftBound;
    private Vector3 rightBound;
    private bool movingRight = true;

    void Start()
    {
        SetBounds();
        StartCoroutine(Guard());
        InvokeRepeating(nameof(ShootLaser), shootInterval, shootInterval);
    }

    void SetBounds()
    {
        leftBound = goal.position + Vector3.left * guardSpace + Vector3.down * frontSpace;
        rightBound = goal.position + Vector3.right * guardSpace + Vector3.down * frontSpace;
    }

    IEnumerator Guard()
    {
        while (true)
        {
            if (movingRight)
            {
                transform.position = Vector3.MoveTowards(transform.position, rightBound, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, rightBound) < 0.01f)
                {
                    movingRight = false;
                    yield return new WaitForSeconds(waitTime);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, leftBound, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, leftBound) < 0.01f)
                {
                    movingRight = true;
                    yield return new WaitForSeconds(waitTime);
                }
            }

            yield return null;
        }
    }


    void ShootLaser()
    {
        if (puck == null) return;

        
        Vector2 direction = (puck.position - transform.position).normalized;

        
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        laserRb.velocity = direction * laserSpeed;

        
        Collider2D enemyCollider = GetComponent<Collider2D>();
        Collider2D laserCollider = laser.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(laserCollider, enemyCollider);
    }

    void OnDrawGizmos()
    {
        if (goal == null) return;

        
        SetBounds();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(leftBound, rightBound);
    }
}
