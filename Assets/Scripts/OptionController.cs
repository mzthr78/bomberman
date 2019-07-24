using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionController : MonoBehaviour
{
    public Button Return2TitleButton;

    void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
