using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { set; get; }

    public string firstScene;
    [HideInInspector]
    public string currentScene;
    string QueuedScene;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Debug.Log("Hi");
        LoadScene("PlayerScene");
        LoadScene(firstScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading scene: " + sceneName);
        StartCoroutine(FadeInAndLoad(sceneName));
    }

    public void UnloadScene(string sceneName)
    {
        Debug.Log("Unloading scene: " + sceneName);
        SceneManager.UnloadSceneAsync(sceneName);
    }
    IEnumerator FadeInAndLoad(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);
        currentScene = sceneName;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        if (sceneName == "PlayerScene")
        {
            CameraMovement.Instance.player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(CameraMovement.Instance.player);
        }
    }
}
