using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player in the overworld. Handles interactions
// inherits from PlayerTopDown to recieve footstep and movement functionality
public class PlayerOverworld : PlayerTopDown
{
    [Header("Interactions")]
    public float interactRange = 5f;
    public LayerMask interactLayer;

    private OverworldInteractable closestInteractable;
    private OverworldInteractable previousInteractable;

    new protected void Update()
    {
        base.Update();
        FindClosestInteractable();
    }

    public void OnSelect()
    {
        if (closestInteractable != null)
        {
            closestInteractable.OnSelect();
        }
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
