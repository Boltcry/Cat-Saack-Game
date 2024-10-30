using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderSorter : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private bool isMoving = false;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        SortSpriteOrder();
    }

    void Update()
    {
        if (isMoving)
        {
            SortSpriteOrder();
        }
    }

    void SortSpriteOrder()
    {
        spriteRenderer.sortingOrder = Mathf.FloorToInt(-transform.position.y * 10);
    }
}