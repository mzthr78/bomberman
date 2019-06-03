﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public Text Platform;

    public Button StartButton;
    public Button RankingButton;
    public Button OptionButton;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(() => LoadLoadScene());
        RankingButton.onClick.AddListener(() => LoadRankingScene());
        OptionButton.onClick.AddListener(() => LoadOptionScene());

        Platform.text = "";
        Platform.text = Application.platform.ToString();

    }

    void LoadLoadScene()
    {
        SceneManager.LoadScene("LoadScene");
    }

    void LoadRankingScene()
    {
        SceneManager.LoadScene("RankingScene");
    }

    void LoadOptionScene()
    {
        SceneManager.LoadScene("OptionScene");
    }
}
