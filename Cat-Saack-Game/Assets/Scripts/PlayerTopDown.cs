using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDown : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    private float xInput = 0;
    private float yInput = 0;

    [Header("Footstep Audio")]
    public AudioClip[] footstepAudioClips;
    public float footstepInterval = 0.7f;
    public float walkSpeedThreshold = 0.1f;
    private float footstepTimer = 0f;

    protected Animator anim;

    protected Rigidbody2D rb;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected void Update()
    {
        HandleFootstepAudio();
    }

    protected void FixedUpdate()
    {
        if (anim != null)
        {
            anim.SetFloat("xInput", xInput);
            anim.SetFloat("yInput", yInput);
        }

        Vector2 moveInput = new Vector2(xInput, yInput);
        moveInput.Normalize();
        rb.velocity = moveInput * moveSpeed;
    }

    // Takes move input from InputManager and moves the player
    public void Move(Vector2 aMoveInput)
    {
        xInput = aMoveInput.x;
        yInput = aMoveInput.y;
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
