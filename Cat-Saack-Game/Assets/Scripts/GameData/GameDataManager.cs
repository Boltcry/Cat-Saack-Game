using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// intended to be attached as a component to the main GameManager object
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public bool tutorialPassed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // should be set by GameManager already
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void SetTutorialPassed(bool aBool)
    {
        Instance.tutorialPassed = aBool;
    }
}
