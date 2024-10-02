using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player in the overworld. Handles movement
public class PlayerOverworld : MonoBehaviour
{

    private Rigidbody2D rb;
    public float moveSpeed = 3f;
    public float interactRange = 5f;
    public LayerMask interactLayer;

    private OverworldInteractable closestInteractable;
    private OverworldInteractable previousInteractable;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FindClosestInteractable();
    }

    public void OnSelect()
    {
        if (closestInteractable != null)
        {
            closestInteractable.OnSelect();
        }
    }

    // Takes move input from InputManager and moves the player
    public void Move(Vector2 aMoveInput)
    {
        //anim.SetFloat("xInput", aMoveInput.x);
        //anim.SetFloat("yInput", aMoveInput.y);

        Vector2 velocity = rb.velocity;
        velocity.x = aMoveInput.x * moveSpeed;
        velocity.y = aMoveInput.y * moveSpeed;
        rb.velocity = velocity;
    }

    // Checks for the closest OverworldInteractable on the Interactable layer to the player and marks it
    void FindClosestInteractable()
    {
        float closestDistance = Mathf.Infinity;
        OverworldInteractable interactable = null;

        foreach(Collider2D each in Physics2D.OverlapCircleAll(transform.position, interactRange, interactLayer))
        {
            OverworldInteractable tempInteractable = each.gameObject.GetComponent<OverworldInteractable>();
            if (tempInteractable != null)
            {
                float distance = Vector3.Distance(transform.position, each.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    interactable = tempInteractable;
                    Debug.Log($"Found interactable: {tempInteractable.gameObject.name} at distance {distance}");
                }
            }
        }
        if (interactable != closestInteractable)
        {
            if(closestInteractable != null)
            {
                closestInteractable.SetOutlineActive(false);
            }
            if (interactable != null)
            {
                interactable.SetOutlineActive(true);
            }
        }
        previousInteractable = closestInteractable;
        closestInteractable = interactable;
    }

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //Show interact range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactRange);
        }
    #endif
}
