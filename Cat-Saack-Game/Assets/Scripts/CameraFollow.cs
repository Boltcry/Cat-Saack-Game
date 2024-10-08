using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.3f;
    private Vector3 refVelocity;
    //public float gameScreenWidth = 5;
    public BoxCollider2D cameraBounds;

    PixelPerfectCamera pixelPerfectCamera;

    void Start()
    {
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();

        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }

    void FixedUpdate()
    {
        
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, target.position, ref refVelocity, smoothTime);

        int pixelsPerUnit = pixelPerfectCamera.assetsPPU;
        // calculate camera size in world units
        float cameraHalfWidth = pixelPerfectCamera.refResolutionX / (2f * pixelsPerUnit);
        float cameraHalfHeight = pixelPerfectCamera.refResolutionY / (2f * pixelsPerUnit);

        if (cameraBounds != null)
        {
            // clamp position in room bounds
            Bounds bounds = cameraBounds.bounds;
            float minX = bounds.min.x + cameraHalfWidth;
            float maxX = bounds.max.x - cameraHalfWidth;
            float minY = bounds.min.y + cameraHalfHeight;
            float maxY = bounds.max.y - cameraHalfHeight;

            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        }

        // pixels snap
        float snappedX = Mathf.Round(targetPosition.x * pixelsPerUnit) / pixelsPerUnit;
        float snappedY = Mathf.Round(targetPosition.y * pixelsPerUnit) / pixelsPerUnit;

        transform.position = new Vector3(snappedX, snappedY, -10f);
    }

    #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (cameraBounds != null)
            {
                Gizmos.color = Color.yellow;
                Bounds bounds = cameraBounds.bounds;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }
    #endif
}