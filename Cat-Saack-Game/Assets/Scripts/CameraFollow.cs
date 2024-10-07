using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.3f;
    private Vector3 refVelocity;
    public float gameScreenWidth = 5;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, target.position, ref refVelocity, smoothTime);
        
        transform.position = new Vector3(Mathf.Clamp(targetPosition.x, -gameScreenWidth, gameScreenWidth), targetPosition.y, -10);
    }
}