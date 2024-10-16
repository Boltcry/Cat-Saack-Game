using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float spawnChance;

    Collider2D triggerCollider;
    CollectibleSpawner spawner;

    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    protected virtual void OnCollect()
    {
        Debug.Log("Collectible collected: "+this);
        // play collection animation
        // add points, or apply powerup, or whateverI 
        DestroyCollectible();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect();
        }
    }

    void DestroyCollectible()
    {
        spawner.DeregisterCollectible();
        bool bankSuccessful = ItemBankManager.AddObjectToBank(this.gameObject);
        if (!bankSuccessful)
        {
            Destroy(this.gameObject);
        }
    }

    public void RegisterSpawner(CollectibleSpawner aSpawner)
    {
        spawner = aSpawner;
    }
}
