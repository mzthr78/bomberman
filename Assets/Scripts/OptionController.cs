using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionController : MonoBehaviour
{
    public Button OKButton;
    public Button CancelButton;

    public Button NormalModeButton;
    public Button NamepuModeButton;

    GameMode tmpMode;

    public InputField SoftBlockNumText;

    private void Start()
    {
        OKButton.onClick.AddListener(() => SetOptions());
        CancelButton.onClick.AddListener(() => LoadTitleScene());
        tmpMode = GameController.GetGameMode();

        if (tmpMode == GameMode.Namepu)
        {
            NamepuModeButton.Select();
        }
        else
        {
            NormalModeButton.Select();
        }

        NormalModeButton.onClick.AddListener(() => GameModeOnClick(GameMode.Normal));
        NamepuModeButton.onClick.AddListener(() => GameModeOnClick(GameMode.Namepu));
        SoftBlockNumText.text = GameController.GetSoftBlockNum().ToString();
    }

    void GameModeOnClick(GameMode mode)
    {
        tmpMode = mode;

        ColorBlock selectedColors = new ColorBlock
        {
            normalColor = new Color(0, 255, 0)
        };


        ColorBlock normalColors = new ColorBlock
        {
            normalColor = new Color(255, 255, 255)
        };

        switch (mode)
        {
            case GameMode.Namepu:
                break;
            default: //normal
                break;
        }
    }

    void SetOptions()
    {
        GameController.SetSoftBlockNum(int.Parse(SoftBlockNumText.text));
        GameController.SetGameMode(tmpMode);

        Debug.Log("mode=" + tmpMode);

        LoadTitleScene();
    }

    void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
