using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCSCENE : MonoBehaviour
{
    public string targetScene;
  
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayTime();
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

}
