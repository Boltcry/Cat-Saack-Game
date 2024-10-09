using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// player controller for DuckDodgeInfinite minigame.
// inherits from PlayerTopDown to recieve footstep and movement functionality
public class PlayerDuckDodgeInfinite : PlayerTopDown
{
    public SpriteRenderer sprite;
    public SpriteRenderer invincibilitySprite;

    [Header("Game Variables")]
    public int health = 3;
    bool isInvincible = false;
    float invincibilityTimeLeft = 0.0f;

    private Vector3 startPosition;

    new void Start()
    {
        base.Start();
        startPosition = transform.position;
        isInvincible = false; // reset upon starting new game
        
        // get sprite & invincibilitySprite if not already set
        if (sprite == null || invincibilitySprite == null)
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                if (sr.gameObject.name.Equals("Sprite", StringComparison.OrdinalIgnoreCase))
                {
                    sprite = sr;
                }
                else if (sr.gameObject.name.Equals("InvincibilitySprite", StringComparison.OrdinalIgnoreCase))
                {
                    invincibilitySprite = sr;
                }
            }
        }
    }

    new void Update()
    {
        base.Update();

        if (MinigameManagerDuck.gameIsRunning)
        {
            // gameplay update function
            if (isInvincible)
            {
                invincibilityTimeLeft -= Time.deltaTime;

                if (invincibilityTimeLeft <= 0.0f)
                {
                    isInvincible = false;
                    invincibilitySprite.gameObject.SetActive(false);
                }
            }
        }
    }

    public void TakeHit()
    {
        if(!isInvincible)
        {
            health--;
            SetInvincible(1f);
            if(health<=0)
            {
                MinigameManagerDuck.GameOver();
            }
        }
    }

    public void SetInvincible(float duration)
    {
        isInvincible = true;
        invincibilitySprite.gameObject.SetActive(true);
        invincibilityTimeLeft = duration;
    }

    public int GetHealth()
    {
        return health;
    }
}
