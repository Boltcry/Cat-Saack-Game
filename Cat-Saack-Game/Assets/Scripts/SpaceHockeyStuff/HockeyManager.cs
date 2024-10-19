using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HockeyManager : MonoBehaviour
{
    public static HockeyManager Instance;
   void Awake()
   {
    if (Instance == null)
    {
        Instance = this;
    }
    else
    {
        Destroy(gameObject);
    }
   }

   public void HomeScore()
   {
    Debug.Log("home score +1");
   }

   public void AwayScore()
   {
    Debug.Log("Away score +1");
   }

   public void ResetPlayer()
   {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.transform.position = new Vector2(0f, -3.69f);
        Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
   }
}
