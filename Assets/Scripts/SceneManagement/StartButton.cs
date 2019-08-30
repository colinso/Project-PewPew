using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Update is called once per frame
    private void OnMouseDown()
    {
        SceneManager.LoadScene("MainScene");
        SceneManager.UnloadSceneAsync("StartMenu");
    }
}
