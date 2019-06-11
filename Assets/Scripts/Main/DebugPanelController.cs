using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanelController : MonoBehaviour
{
    public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        SetText("abc");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string s)
    {
        text.GetComponent<TextMesh>().text = s;
    }
}
