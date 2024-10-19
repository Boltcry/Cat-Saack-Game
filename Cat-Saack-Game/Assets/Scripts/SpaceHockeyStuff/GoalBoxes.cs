using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBoxes : MonoBehaviour
{
    public string team;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Puck"))
        {
            if (team == "Home")
            {
                HockeyManager.Instance.HomeScore();
            }

            else  if (team == "Away")
            {
                HockeyManager.Instance.AwayScore();
            }

            ResetPuck();
            HockeyManager.Instance.ResetPlayer();
        }
    }



    void ResetPuck()
    {
        GameObject puck = GameObject.FindGameObjectWithTag("Puck");
        puck.transform.position = new Vector2(.8f, .18f);
        Rigidbody2D rb = puck.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;

    }
}

