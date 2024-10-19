using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMovement : MonoBehaviour
{
    public float acceleration = 10f;   
    public float maxSpeed = 5f;        
    public float dashForce = 300f;     
    public float dashCooldown = 1f;    
    private Rigidbody2D rb;
    private Vector2 moveInput = Vector2.zero;
    private bool canDash = true;       

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        ApplyMovement(moveInput); 


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && moveInput != Vector2.zero)
        {
            Debug.Log("Dash Pressed");
            Dash();
        }


        //Debug.Log("speed: " + rb.velocity.magnitude);
    }

    void ApplyMovement(Vector2 input)
    {
        if (input != Vector2.zero)
        {
            // Apply force for movement
            rb.AddForce(input.normalized * acceleration);
        }

        // Cap the spaceship's speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Dash()
    {
        rb.AddForce(moveInput.normalized * dashForce, ForceMode2D.Impulse);
        canDash = false; 
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
