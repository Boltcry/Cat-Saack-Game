using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CabInteractables : MonoBehaviour
{

    public string targetScene;
    public float interactRange = 5f;
    public LayerMask playerLayer;

    private Transform playerTransform;
    void Update()
    {
         CheckPlayerInteraction();
    }

    private void CheckPlayerInteraction()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, interactRange, playerLayer);
        if (playerCollider != null)
        {
            if (playerCollider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
             Debug.Log("Selected Cabinet");
                PlayTime();
            }
        }
    }

    public void PlayTime()
    {
        Debug.Log("loading level");
        StartCoroutine(WaitToLoadScene(targetScene));
    }

    private IEnumerator WaitToLoadScene(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }


    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,interactRange);
    }
    #endif

}
