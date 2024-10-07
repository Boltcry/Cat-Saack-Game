using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDown : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Footstep Audio")]
    public AudioClip[] footstepAudioClips;
    public float footstepInterval = 0.7f;
    public float walkSpeedThreshold = 0.1f;
    private float footstepTimer = 0f;

    //protected Animator anim;

    protected Rigidbody2D rb;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
    }

    protected void Update()
    {
        HandleFootstepAudio();
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

    // plays a sound effect for the player's footsteps at a set interval
    void HandleFootstepAudio()
    {
        if (footstepAudioClips.Length > 0)
        {
            float speed = rb.velocity.magnitude;
            // player is moving
            if (speed > walkSpeedThreshold)
            {
                footstepTimer += Time.deltaTime;

                if (footstepTimer >= footstepInterval)
                {
                    AudioManager.PlayAudioClip(AudioType.SFX, footstepAudioClips[Random.Range(0, footstepAudioClips.Length)]);
                    footstepTimer = 0f;
                }
            }
            // player is not moving
            else
            {
                footstepTimer = 0f;
            }
        }
    }
}
