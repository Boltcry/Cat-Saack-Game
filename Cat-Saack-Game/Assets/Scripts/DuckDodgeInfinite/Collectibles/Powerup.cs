using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : Collectible
{
    [SerializeReference]
    public PowerupData powerupType;

    protected override void OnCollect()
    {
        powerupType.ApplyPowerup();
        base.OnCollect();
    }
}

[System.Serializable]
public class PowerupData
{
    public virtual void ApplyPowerup() {}
}

[System.Serializable]
public class SpeedupPowerup : PowerupData
{
    public float duration = 4;
    public float speedMultiplier = 2;

    public override void ApplyPowerup()
    {
        MinigameManagerDuck.SetPlayerSpeed(duration, speedMultiplier);
    }
}

[System.Serializable]
public class InvinciblePowerup : PowerupData
{
    public float duration = 4;

    public override void ApplyPowerup()
    {
        MinigameManagerDuck.SetPlayerInvincible(duration);
    }
}

[System.Serializable]
public class HealthupPowerup : PowerupData
{
    public int healthToAdd = 1;

    public override void ApplyPowerup()
    {
        MinigameManagerDuck.AddPlayerHealth(healthToAdd);
    }
}
