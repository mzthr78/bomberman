using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("GameScene");
    }
}
