using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSecretSceneSkipper : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            LoadSceneManager.Instance.Unload(LoadSceneManager.Instance.currentScene);
            LoadSceneManager.Instance.Load("Scene01", 0);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            LoadSceneManager.Instance.Unload(LoadSceneManager.Instance.currentScene);
            LoadSceneManager.Instance.Load("Scene02", 1);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            LoadSceneManager.Instance.Unload(LoadSceneManager.Instance.currentScene);
            LoadSceneManager.Instance.Load("Scene03", 2);
        }
    }
}
