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
    [Tooltip("Particle system that plays on OnCollect")]
    [SerializeField]
    ParticleSystem collectParticle;

    Collider2D triggerCollider;
    CollectibleSpawner spawner;
    Animator animator;

    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    protected virtual IEnumerator OnCollect()
    {
        //Debug.Log("Collectible collected: "+this);

        // play collection sound
        if (collectSound != null)
        {
            AudioManager.PlayAudioClip(AudioType.SFX, collectSound);
        }

        // play collection animation
        if (collectParticle != null)
        {
            // disable sprite
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                sprite.enabled = false;
            }

            collectParticle.Play();
            yield return StartCoroutine(WaitForParticles());
        }

        // add points, or apply powerup, or whateverI 
        if (incrementTokenCount)
        {
            MinigameManagerDuck.IncrementTokensCollected();
        }

        if (!collectAnimationName.Equals(""))
        {
            yield return StartCoroutine(WaitForCloseAnimation());
        }

        DestroyCollectible();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(OnCollect());
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

    private IEnumerator WaitForCloseAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("shouldClose", true);

            // wait for animation to start and finish
            yield return new WaitUntil(() => 
                animator.GetCurrentAnimatorStateInfo(0).IsName(collectAnimationName));
        }
    }

    private IEnumerator WaitForParticles()
    {
        while (collectParticle.isPlaying)
        {
            yield return null;
        }
    }

    public void RegisterSpawner(CollectibleSpawner aSpawner)
    {
        spawner = aSpawner;
    }
}
