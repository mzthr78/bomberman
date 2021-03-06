﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearSceneController : MonoBehaviour
{
    public GameObject InputPanelPrefab;
    public GameObject Canvas;

    float delta = 0;
    float span = 3;

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if (delta > span)
        {
            delta = 0;
            GameObject InputPanel = Instantiate(InputPanelPrefab, Canvas.transform);
        }
    }
}
