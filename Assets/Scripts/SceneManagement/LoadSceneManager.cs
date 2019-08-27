using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { set; get; }

    public string firstScene;
    [HideInInspector]
    public string currentScene;
    public Animator blackScreenAnimator;
    public Image blackScreenImage;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartCoroutine(LoadFirstScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadFirstScene()
    {
        yield return new WaitUntil(() => blackScreenImage.color.a == 1);
        SceneManager.LoadScene("PlayerScene", LoadSceneMode.Additive);
        yield return new WaitUntil(() => SceneManager.GetSceneByName("PlayerScene").isLoaded);
        CameraMovement.Instance.player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.LoadScene(firstScene, LoadSceneMode.Additive);
        currentScene = firstScene;
        yield return new WaitUntil(() => SceneManager.GetSceneByName(firstScene).isLoaded);
        blackScreenAnimator.SetTrigger("FadeIn");
    }
    public void Load(string sceneName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Loading scene: " + sceneName);
        StartCoroutine(FadeInAndLoad(sceneName));
    }

    public void Unload(string sceneName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Unloading scene: " + sceneName);
        StartCoroutine(FadeOutAndUnload(sceneName));
    }
    IEnumerator FadeInAndLoad(string sceneName)
    {
        string oldScene = currentScene;
        yield return new WaitUntil(() => blackScreenImage.color.a == 1);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);
        currentScene = sceneName;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        yield return new WaitUntil(() => !SceneManager.GetSceneByName(oldScene).isLoaded && SceneManager.GetSceneByName(sceneName).isLoaded);
        blackScreenAnimator.SetTrigger("FadeIn");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().enabled = true;
    }
    IEnumerator FadeOutAndUnload(string sceneName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().enabled = false;
        blackScreenAnimator.SetTrigger("FadeOut");
        yield return new WaitUntil(() => blackScreenImage.color.a == 1);
        SceneManager.UnloadScene(sceneName);
    }
}
