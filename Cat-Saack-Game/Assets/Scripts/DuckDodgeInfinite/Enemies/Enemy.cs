using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1;

    protected Transform lookTarget;
    protected Vector3 startPosition;
    protected Vector3 movementDirection = Vector3.zero;

    protected Rigidbody2D rb;
    protected Collider2D coll;

    private float destroyDistance = 50f;

    virtual protected void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        startPosition = transform.position;
    }
    
    virtual protected void Update() 
    {
        if (MinigameManagerDuck.gameIsRunning)
        {
            // destroy if too far away
            // later maybe change this to move enemies into a buffer
            BoxCollider2D gameRoomBounds = MinigameManagerDuck.Instance.gameRoomBounds;
            if(Vector3.Distance(transform.position, gameRoomBounds.bounds.center) > destroyDistance) 
            {
                DestroyEnemy();
            }

            // move towards the enemy's movementDirection
            rb.velocity = movementDirection * moveSpeed;
        }
    }

    protected void LookToDirection(Vector3 aDirection)
    {
        if (aDirection != Vector3.zero)
        {
            float lookAngle = (Mathf.Atan2(aDirection.y, aDirection.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler( 0, 0, lookAngle);
        }
    }

    public void SetLookTarget(Transform aTarget)
    {
        if (aTarget != null)
        {
            lookTarget = aTarget;
        }
    }

    // bank the enemy if possible, if not destroy the object
    public void DestroyEnemy()
    {
        bool bankSuccessful = ItemBankManager.AddObjectToBank(this.gameObject);
        if (!bankSuccessful)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetDestroyDistance(float aDistance)
    {
        destroyDistance = aDistance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerDuckDodgeInfinite>().TakeHit();
        }
    }
}
