using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public void LoadLevel(string name)
    {
        Debug.Log("Attempting to load scene \"" + name + "\""); 
        SceneManager.LoadScene(name); 
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game..."); 
        Application.Quit(); 
    }
}
