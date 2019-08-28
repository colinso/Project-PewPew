using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public int startPointNumber;
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
