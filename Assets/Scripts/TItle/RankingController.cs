using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingController : MonoBehaviour
{
    public Button Return2TitleButton;

    // Start is called before the first frame update
    void Start()
    {
        Return2TitleButton.onClick.AddListener(() => LoadTitleScene());
    }

    void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
