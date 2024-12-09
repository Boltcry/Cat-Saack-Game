using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HockeyManager : LevelManager
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

   public void ResetEnemy()
   {
    GameObject Enemy = GameObject.FindGameObjectWithTag("Enemy");
    Enemy.transform.position = new Vector2(0.03f, 3.85f);
    Rigidbody2D erb = Enemy.GetComponent<Rigidbody2D>();
    erb.velocity = Vector2.zero;

   }
}
