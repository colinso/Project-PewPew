﻿using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            NavMeshAgent2D g = GetComponent<NavMeshAgent2D>();
            GetComponent<NavMeshAgent2D>().destination = w;
        }
    }
}