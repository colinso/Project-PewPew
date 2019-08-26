using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
    public static CameraMovement Instance { set; get; }

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if(player)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
