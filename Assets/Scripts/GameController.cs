using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text MousePosition;
    public GameObject stage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            MousePosition.text = "(" + hit.point.x + ", " + hit.point.z + ")";
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("ClearScene");
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("GameOverScene");
        }

        
    }
}
