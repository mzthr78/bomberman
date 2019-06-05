using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadController : MonoBehaviour
{
    public Text StageNumText;

    void Start()
    {
        StageNumText.text = GameController.NextStageNum().ToString();

        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("GameScene");
    }
}
