using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
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

    public void LoadSceneAfterCountry()
    {
        if (Globals.countryNameValid)
        {
            // Current scene
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Next scene
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}
