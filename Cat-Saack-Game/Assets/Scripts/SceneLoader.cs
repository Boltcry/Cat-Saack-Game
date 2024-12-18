using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void StartGoToScene(string aTargetScene)
    {
        Instance.StartCoroutine(GoToScene(aTargetScene));
    }

    // called by either SceneLoadStep or UnityEvents (MenuButton)
    // loads targetScene asynchronously if in the project
    public static IEnumerator GoToScene(string aTargetScene)
    {
        if (SceneInProject(aTargetScene))
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(aTargetScene);

            while (!operation.isDone)
            {
                yield return null;
            }
            InputManager.RegisterSelf();
            Debug.Log("Loaded Scene" +aTargetScene);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(aTargetScene));
        }
    }

    // Checks if a scene of the given name exists in the project
    public static bool SceneInProject(string aSceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameFromPath.Equals(aSceneName))
            {
                return true; // scene found
            }
        }
        Debug.LogWarning("Scene by name " +aSceneName+ " not found.");
        return false; //scene not found
    }
}
