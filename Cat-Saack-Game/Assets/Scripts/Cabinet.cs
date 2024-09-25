using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Cabinet : OverworldInteractable
{
    
    new public void OnSelect()
    {
        Debug.Log("selecint cab?");
        LoadScene();
       Debug.Log("Cabinet Chosen!");
    }



 private void LoadScene()
 {
    SceneManager.LoadScene(2);
 }
}
