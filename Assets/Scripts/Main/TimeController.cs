using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    GameController controller;

    Text timeText;
    public float time = 200;

    bool freeze = false;

    public void Freeze(bool b = true)
    {
        this.freeze = b;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        timeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.IsFreeze()) return;

        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            controller.Death();
        }
        timeText.text = Mathf.RoundToInt(time).ToString();
    }
}
