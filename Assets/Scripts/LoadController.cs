using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("GameScene");
    }
}
