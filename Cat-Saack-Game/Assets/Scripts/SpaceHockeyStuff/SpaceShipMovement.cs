using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMovement : MonoBehaviour
{
    public float acceleration = 10f;   
    public float maxSpeed = 5f;        
    public float dashForce = 300f;     
    public float dashCooldown = 1f;    
    public float shootCooldown = 1f; 
    private Rigidbody2D rb;
    private Vector2 moveInput = Vector2.zero;
    private bool canDash = true;       
    public GameObject lazerFab;
    public float lazerSpeed = 10f;
    private bool canShoot = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        ApplyMovement(moveInput); 
        MouseAim();


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && moveInput != Vector2.zero)
        {
            Debug.Log("Dash Pressed");
            Dash();
        }

        if (Input.GetMouseButtonDown(0) && canShoot) 
        {
            Lazer();
        }


        //Debug.Log("speed: " + rb.velocity.magnitude);
    }

    void Lazer()
    {
        GameObject lazer = Instantiate(lazerFab, transform.position, transform.rotation);

        Rigidbody2D lazerRb = lazer.GetComponent<Rigidbody2D>();
        lazerRb.velocity = transform.up * lazerSpeed; // This should keep the lazer in line with the ship rotation. 

        Collider2D playerCollider = GetComponent<Collider2D>();
        Collider2D lazerCollider = lazer.GetComponent<Collider2D>(); //This should collect both colliders

        Physics2D.IgnoreCollision(lazerCollider, playerCollider);
        canShoot = false;
        StartCoroutine(ShootCoolDown());

    }

    void MouseAim()
    {
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 direction = (mousePosition - transform.position).normalized;

    // Rotate the spaceship to face the mouse position
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
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
        StartCoroutine(DashCoolDown());
    }

    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator ShootCoolDown()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
