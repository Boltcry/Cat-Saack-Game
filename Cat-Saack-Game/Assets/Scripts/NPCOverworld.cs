using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCOverworld : OverworldInteractable
{
    new public void OnSelect()
    {
        Debug.Log("npc selected");
        base.OnSelect();
    }
}
