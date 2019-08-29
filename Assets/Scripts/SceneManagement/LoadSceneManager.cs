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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CameraMovement.Instance.player = player;
        SceneManager.LoadScene(firstScene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => SceneManager.GetSceneByName(firstScene).isLoaded);
        currentScene = firstScene;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
        placePlayer(player.GetComponent<PlayerController>(), 0);
        blackScreenAnimator.SetTrigger("FadeIn");
    }
    public void Load(string sceneName, int doorNumber)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Loading scene: " + sceneName);
        StartCoroutine(FadeInAndLoad(sceneName, doorNumber));
    }

    public void Unload(string sceneName)
    {
        Debug.Log("Unloading scene: " + sceneName);
        StartCoroutine(FadeOutAndUnload(sceneName));
    }
    IEnumerator FadeInAndLoad(string sceneName, int doorNumber)
    {
        string oldScene = currentScene;

        yield return new WaitUntil(() => blackScreenImage.color.a == 1);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        yield return new WaitUntil(() => SceneManager.GetSceneByName(sceneName).isLoaded);

        currentScene = sceneName;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        placePlayer(player.GetComponent<PlayerController>(), doorNumber);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        yield return new WaitUntil(() => ((!SceneManager.GetSceneByName(oldScene).isLoaded || player.GetComponent<PlayerController>().reloading == true) && SceneManager.GetSceneByName(sceneName).isLoaded));

        blackScreenAnimator.SetTrigger("FadeIn");
        player.GetComponent<PlayerMovement>().enabled = true;
    }
    IEnumerator FadeOutAndUnload(string sceneName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<PlayerMovement>().enabled = false;
        blackScreenAnimator.SetTrigger("FadeOut");

        yield return new WaitUntil(() => blackScreenImage.color.a == 1);

        player.GetComponent<PlayerController>().disableColliders();
		SceneManager.UnloadSceneAsync(sceneName);
    }

    private void placePlayer(PlayerController playerController, int doorNumber)
    {
        StartPoint[] startPoints = (StartPoint[])FindObjectsOfType(typeof(StartPoint));

        foreach(StartPoint sp in startPoints)
        {
            if(sp.startPointNumber == doorNumber)
            {
                playerController.transform.position = sp.transform.position;
            }
        }
        playerController.enableColliders();
    }

    public void Reload()
    {
        Debug.Log("Reloading scene");
        StartCoroutine(ReloadScene());
    }
    IEnumerator ReloadScene()
    {
        StartPoint[] startPoints = (StartPoint[])FindObjectsOfType(typeof(StartPoint));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
        yield return new WaitUntil(() => SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainScene"));
        Unload(currentScene);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		yield return new WaitUntil(() => !SceneManager.GetSceneByName(currentScene).isLoaded);
		player.GetComponent<PlayerController>().health = player.GetComponent<PlayerController>().maxHealth;
		yield return new WaitUntil(() => player.GetComponent<PlayerController>().health == player.GetComponent<PlayerController>().maxHealth);
		Load(currentScene, startPoints[0].startPointNumber);
		yield return new WaitUntil(() => SceneManager.GetSceneByName(currentScene).isLoaded);
		player.GetComponent<PlayerController>().reloading = false;
	}
}
