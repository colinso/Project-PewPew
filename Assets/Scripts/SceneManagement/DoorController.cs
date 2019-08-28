using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public string NextSceneName;
    public int doorNumber;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        LoadSceneManager.Instance.Unload(LoadSceneManager.Instance.currentScene);
        LoadSceneManager.Instance.Load(NextSceneName, doorNumber);
    }
}
