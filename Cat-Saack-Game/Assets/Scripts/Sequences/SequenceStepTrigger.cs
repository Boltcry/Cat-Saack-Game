using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SequenceStepTrigger : MonoBehaviour
{
    Collider2D triggerCollider;
    public event Action OnTriggered;

    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggered?.Invoke();
        }
    }
}
