using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get the active scene and its name
        /*string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene: " + currentSceneName);*/
    }

    // Method to load a scene by its name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Loads the scene by name
    }

    // Method to load a scene by its index
    public void LoadSceneByIndex(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex); // Loads the scene by index
    }

    public string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name; // Returns the active scene name
    }
}
