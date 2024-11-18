using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float spawnChance;
    public AudioClip collectSound;
    public bool incrementTokenCount = false;
    [Tooltip("Name of the animation played on collect")]
    public string collectAnimationName;

    Collider2D triggerCollider;
    CollectibleSpawner spawner;
    Animator animator;

    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void OnCollect()
    {
        //Debug.Log("Collectible collected: "+this);

        // play collection sound
        if (collectSound != null)
        {
            AudioManager.PlayAudioClip(AudioType.SFX, collectSound);
        }
        // play collection animation
        // add points, or apply powerup, or whateverI 
        if (incrementTokenCount)
        {
            MinigameManagerDuck.IncrementTokensCollected();
        }

        StartCoroutine(DestroyAfterAnimation());
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

    private IEnumerator DestroyAfterAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("shouldClose", true);

            // wait for animation to start and finish
            yield return new WaitUntil(() => 
                animator.GetCurrentAnimatorStateInfo(0).IsName(collectAnimationName));
        }

        DestroyCollectible();
    }

    public void RegisterSpawner(CollectibleSpawner aSpawner)
    {
        spawner = aSpawner;
    }
}
