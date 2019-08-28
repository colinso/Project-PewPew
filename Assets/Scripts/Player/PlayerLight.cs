using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    public float zDist = -4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, zDist);
    }
}
