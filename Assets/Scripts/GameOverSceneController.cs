using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneController : MonoBehaviour
{
    public GameObject InputPanelPrefab;
    public GameObject Canvas;

    float delta = 0;
    float span = 8;

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;

        if (delta > span)
        {
            delta = 0;
            //Debug.Log("passed!");
            GameObject InputPanel = Instantiate(InputPanelPrefab, Canvas.transform);
        }

        /*
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            SceneManager.LoadScene("TitleScene");
        }
        */
    }
}
