using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        // Current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Next scene
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadIndexScene()
    {
        // First scene
        SceneManager.LoadScene(0);
    }
}
