using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    Text timeText;
    float time = 200;

    bool freeze = false;

    public void Freeze(bool b = true)
    {
        this.freeze = b;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0) time -= Time.deltaTime;
        timeText.text = Mathf.RoundToInt(time).ToString();
    }
}
