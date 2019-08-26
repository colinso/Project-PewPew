﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public string NextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        LoadSceneManager.Instance.Unload(LoadSceneManager.Instance.currentScene);
        LoadSceneManager.Instance.Load(NextSceneName);
    }
}
