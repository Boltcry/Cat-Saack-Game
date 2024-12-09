using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLayout : MonoBehaviour
{
    [SerializeField]
    private Transform playerStartPosition;
    [SerializeField]
    private BoxCollider2D gameRoomBounds;
    [SerializeField]
    private BoxCollider2D cameraBounds;
    [SerializeField]
    private Tilemap walkableTilemap;
    [SerializeField]
    private Tilemap collisionTilemap;

    public Transform GetStartPosition()
    {
        return playerStartPosition;
    }

    public BoxCollider2D GetGameRoomBounds()
    {
        return gameRoomBounds;
    }

    public BoxCollider2D GetCameraBounds()
    {
        return cameraBounds;
    }

    public Tilemap GetWalkableTilemap()
    {
        return walkableTilemap;
    }

    public Tilemap GetCollisionTilemap()
    {
        return collisionTilemap;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // draw player start position
        if (playerStartPosition != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerStartPosition.position, 0.3f);
        }
        // draw gameRoomBounds
        if (gameRoomBounds != null)
        {
            Bounds bounds = gameRoomBounds.bounds;
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
#endif
}
