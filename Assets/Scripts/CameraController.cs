﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tmp = target.position + offset;
        transform.position = new Vector3(tmp.x, tmp.y, 0);
    }
}
